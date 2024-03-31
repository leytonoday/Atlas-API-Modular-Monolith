using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Atlas.Shared.Infrastructure.Module;


/// <summary>
/// Represents the startup interface for a module.
/// </summary>
public interface IModuleStartup
{
    /// <summary>
    /// Starts the module, and hooks up all services to the dependency injection container.
    /// </summary>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="eventBus">The event bus for publishing events.</param>
    /// <param name="loggerFactory">The logger factory used to create concrete instances of <see cref="ILogger"/>.</param>
    /// <param name="enableScheduler">Flag indicating whether to enable the scheduler.</param>
    public static abstract Task Start(IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory, bool enableScheduler = true);

    /// <summary>
    /// Stops the module.
    /// </summary>
    public static abstract Task Stop();

    /// <summary>
    /// Sets up scheduled jobs for the module.
    /// </summary>
    /// <returns>The scheduler instance.</returns>
    protected static abstract Task<IScheduler> SetupScheduledJobs();

    protected static abstract void SetupCompositionRoot(IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory);
}