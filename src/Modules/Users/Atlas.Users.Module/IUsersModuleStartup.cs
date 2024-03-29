using Atlas.Shared.Module;
using Microsoft.Extensions.Configuration;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Reflection;

namespace Atlas.Users.Module;

public class UsersModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    /// <inheritdoc />
    public static async Task Start(IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, bool enableScheduler = true)
    {
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        UsersCompositionRoot.SetProvider(serviceProvider);

        if (enableScheduler)
        {
            await SetupScheduledJobs();
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
