using Microsoft.Extensions.Configuration;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Reflection;
using Atlas.Users.Infrastructure;
using Atlas.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.BackgroundJobs;
using Atlas.Shared.Infrastructure.Integration;

namespace Atlas.Users.Module;

public class UsersModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    /// <inheritdoc />
    public static async Task Start(IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory, bool enableScheduler = true)
    {
        SetupCompositionRoot(configuration, eventBus, loggerFactory);

        UsersEventBusStartup.Initialize(loggerFactory.CreateLogger<UsersEventBusStartup>(), eventBus);

        if (enableScheduler)
        {
            _scheduler = await SetupScheduledJobs();
        }
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

    public static void SetupCompositionRoot(IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
    {
        var serviceProvider = new ServiceCollection()
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(eventBus)
            .AddSingleton(loggerFactory)
            .BuildServiceProvider();

        UsersCompositionRoot.SetProvider(serviceProvider);
    }
}
