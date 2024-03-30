using Atlas.Shared.Module;
using Microsoft.Extensions.Configuration;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Reflection;
using Atlas.Users.Application;
using Atlas.Users.Infrastructure;
using Atlas.Shared.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Atlas.Users.Module;

public class UsersModuleStartup : IModuleStartup
{
    private static IScheduler? _scheduler;

    /// <inheritdoc />
    public static async Task Start(IExecutionContextAccessor? executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory, bool enableScheduler = true)
    {
        var usersApplicationAssembly = typeof(UsersApplicationAssemblyReference).Assembly;
        var usersInfrastructureAssembly = typeof(UsersInfrastructureAssemblyReference).Assembly;

        var serviceProvider = new ServiceCollection()
            .AddCommon(configuration)
            .AddValidation(usersApplicationAssembly)
            .AddAutoMappings(usersInfrastructureAssembly)
            .AddServices(configuration)
            .AddSingleton(eventBus)
            .AddSingleton(loggerFactory)
            .BuildServiceProvider();

        UsersCompositionRoot.SetProvider(serviceProvider);

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
