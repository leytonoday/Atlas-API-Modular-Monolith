using Microsoft.Extensions.Configuration;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.BackgroundJobs;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Users.Infrastructure.Persistance;
using Atlas.Shared.Application.Abstractions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Atlas.Users.Infrastructure.Extensions;
using Atlas.Shared.Infrastructure.Extensions;
using Atlas.Shared.Application.ModuleBridge;

namespace Atlas.Users.Infrastructure.Module;

public class UsersModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    /// <inheritdoc />
    public static async Task Start(IModuleBridge moduleBridge, IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
    {
        SetupCompositionRoot(moduleBridge, executionContextAccessor, configuration, eventBus, loggerFactory);

        UsersEventBusStartup.Initialize(loggerFactory.CreateLogger<UsersEventBusStartup>(), eventBus);
        
        _scheduler = await SetupScheduledJobs();
    }

    /// <inheritdoc />
    public static async Task Stop()
    {
        if (_scheduler is not null)
        {
            await _scheduler.Shutdown();
        }
    }

    /// <inheritdoc />
    public static async Task<IScheduler> SetupScheduledJobs()
    {
        var factory = new StdSchedulerFactory(new NameValueCollection
        {
            { "quartz.scheduler.instanceName", Assembly.GetExecutingAssembly().GetName().Name }
        });

        var scheduler = await factory.GetScheduler();

        await scheduler.AddMessageboxJob<ProcessInboxJob<UsersCompositionRoot>>();
        await scheduler.AddMessageboxJob<ProcessOutboxJob<UsersCompositionRoot>>();
        await scheduler.AddMessageboxJob<ProcessQueueJob<UsersCompositionRoot>>();

        await scheduler.Start();

        return scheduler;
    }

    /// <inheritdoc />
    public static void SetupCompositionRoot(IModuleBridge moduleBridge, IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IExecutionContextAccessor>(_ => executionContextAccessor)
            .AddScoped<IModuleBridge>(_ => moduleBridge)
            .AddCommon<UsersDatabaseContext, UsersCompositionRoot>(configuration)
            .AddServices(configuration)
            .AddSingleton(eventBus)
            .AddSingleton(loggerFactory);

        var containerBuilder = new ContainerBuilder();

        containerBuilder.Populate(serviceProvider);

        containerBuilder.AddAutofacServices();

        var container = containerBuilder.Build();

        UsersCompositionRoot.SetContainer(container);
    }
}
