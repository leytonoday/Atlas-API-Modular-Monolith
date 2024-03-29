using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Atlas.Shared.Module;


/// <summary>
/// Represents the startup interface for a module.
/// </summary>
public interface IModuleStartup
{
    /// <summary>
    /// Starts the module, and hooks up all services to the dependency injection container.
    /// </summary>
    /// <param name="executionContextAccessor">The execution context accessor.</param>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="eventBus">The event bus for publishing events.</param>
    /// <param name="enableScheduler">Flag indicating whether to enable the scheduler.</param>
    public static abstract Task Start(IExecutionContextAccessor executionContextAccessor, IConfiguration configuration, IEventBus eventBus, bool enableScheduler = true);

    /// <summary>
    /// Stops the module.
    /// </summary>
    public static abstract Task Stop();

    /// <summary>
    /// Sets up scheduled jobs for the module.
    /// </summary>
    /// <returns>The scheduler instance.</returns>
    protected static abstract Task<IScheduler> SetupScheduledJobs();
}