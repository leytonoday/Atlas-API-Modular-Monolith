using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Shared.Infrastructure.BackgroundJobs;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Shared.Infrastructure.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Reflection;
using Atlas.Shared.Infrastructure.Integration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Atlas.Shared.Infrastructure.Extensions;
using Atlas.Law.Infrastructure.Persistance;
using Atlas.Law.Infrastructure.Extensions;

namespace Atlas.Law.Infrastructure.Module;

public class LawModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    public static async Task Start(IModuleBridge moduleBridge, IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
    {
        SetupCompositionRoot(moduleBridge, executionContextAccessor, configuration, eventBus, loggerFactory);

        LawEventBusStartup.Initialize(loggerFactory.CreateLogger<LawEventBusStartup>(), eventBus);

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
    public static void SetupCompositionRoot(IModuleBridge moduleBridge, IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IExecutionContextAccessor>(_ => executionContextAccessor)
            .AddScoped<IModuleBridge>(_ => moduleBridge)
            .AddCommon<LawDatabaseContext, LawCompositionRoot>(configuration)
            .AddServices(configuration)
            .AddSingleton(eventBus)
            .AddSingleton(loggerFactory);

        var containerBuilder = new ContainerBuilder();

        containerBuilder.Populate(serviceProvider);

        containerBuilder.AddAutofacServices();

        var container = containerBuilder.Build();

        LawCompositionRoot.SetContainer(container);
    }


    /// <inheritdoc />
    public static async Task<IScheduler> SetupScheduledJobs()
    {
        var factory = new StdSchedulerFactory(new NameValueCollection
        {
            { "quartz.scheduler.instanceName", Assembly.GetExecutingAssembly().GetName().Name }
        });

        var scheduler = await factory.GetScheduler();

        await scheduler.AddMessageboxJob<ProcessInboxJob<LawCompositionRoot>>();
        await scheduler.AddMessageboxJob<ProcessOutboxJob<LawCompositionRoot>>();
        await scheduler.AddMessageboxJob<ProcessQueueJob<LawCompositionRoot>>();

        await scheduler.Start();

        return scheduler;
    }
}
