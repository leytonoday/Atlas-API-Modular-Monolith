using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Shared.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz;
using System.Collections.Specialized;
using System.Reflection;
using Atlas.Shared.Infrastructure;
using Atlas.Plans.Infrastructure;
using Atlas.Plans.Application;
using Microsoft.Extensions.Logging;

namespace Atlas.Plans.Module;

public class PlansModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    /// <inheritdoc />
    public static async Task Start(IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory, bool enableScheduler = true)
    {
        var serviceProvider = new ServiceCollection()
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(eventBus)
            .AddSingleton(loggerFactory)
            .BuildServiceProvider();

        PlansCompositionRoot.SetProvider(serviceProvider);

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
        await scheduler.Start();

        return scheduler;
    }
}