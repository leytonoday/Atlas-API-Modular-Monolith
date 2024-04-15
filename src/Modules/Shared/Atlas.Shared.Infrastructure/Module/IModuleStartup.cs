using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.ModuleBridge;
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
    /// Starts the module, hooks up all services to the dependency injection container, and optionally enables the scheduler.
    /// </summary>
    /// <param name="moduleBridge">The module bridge instance for synchronous inter-module communication.</param>
    /// <param name="executionContextAccessor">The execution context accessor for accessing the current execution context.</param>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="eventBus">The event bus for publishing events.</param>
    /// <param name="loggerFactory">The logger factory used to create concrete instances of <see cref="ILogger"/>.</param>
    /// <param name="enableScheduler">Flag indicating whether to enable the scheduler.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static abstract Task Start(IModuleBridge moduleBridge, IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory);

    /// <summary>
    /// Stops the module.
    /// </summary>
    public static abstract Task Stop();

    /// <summary>
    /// Sets up scheduled jobs for the module.
    /// </summary>
    /// <returns>The scheduler instance.</returns>
    protected static abstract Task<IScheduler> SetupScheduledJobs();

    /// <summary>
    /// Sets up the composition root for the module, configuring services and dependencies for this particular module. 
    /// </summary>
    /// <param name="moduleBridge">The module bridge instance for inter-module communication.</param>
    /// <param name="executionContextAccessor">The execution context accessor for accessing the current execution context.</param>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="eventBus">The event bus for publishing events.</param>
    /// <param name="loggerFactory">The logger factory used to create concrete instances of <see cref="ILogger"/>.</param>
    protected static abstract void SetupCompositionRoot(IModuleBridge moduleBridge, IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, ILoggerFactory loggerFactory);
}