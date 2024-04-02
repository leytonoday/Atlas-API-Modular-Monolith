using Atlas.Shared.Infrastructure.Integration.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz;
using System.Collections.Specialized;
using System.Reflection;
using Atlas.Shared.Infrastructure;
using Atlas.Plans.Infrastructure;
using Microsoft.Extensions.Logging;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Shared.Infrastructure.BackgroundJobs;
using Atlas.Plans.Infrastructure.Persistance;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Module;

public class PlansModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    /// <inheritdoc />
    public static async Task Start(IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory, bool enableScheduler = true)
    {
        SetupCompositionRoot(executionContextAccessor, configuration, eventBus, loggerFactory);

        PlansEventBusStartup.Initialize(loggerFactory.CreateLogger<PlansEventBusStartup>(), eventBus);

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

        await scheduler.AddMessageboxJob<ProcessInboxJob<PlansCompositionRoot>>();
        await scheduler.AddMessageboxJob<ProcessOutboxJob<PlansCompositionRoot>>();
        await scheduler.AddMessageboxJob<ProcessQueueJob<PlansCompositionRoot>>();

        await scheduler.Start();

        return scheduler;
    }

    public static void SetupCompositionRoot(IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory)
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IExecutionContextAccessor>(_ => executionContextAccessor)
            .AddCommon<PlansDatabaseContext, PlansCompositionRoot>(configuration)
            .AddServices(configuration)
            .AddSingleton(eventBus)
            .AddSingleton(loggerFactory)
            .BuildServiceProvider();

        PlansCompositionRoot.SetProvider(serviceProvider);
    }
}