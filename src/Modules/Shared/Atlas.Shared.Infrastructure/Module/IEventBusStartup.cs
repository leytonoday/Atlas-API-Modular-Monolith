using Atlas.Shared.Infrastructure.Integration.Bus;
using Microsoft.Extensions.Logging;

namespace Atlas.Shared.Infrastructure.Module;

/// <summary>
/// Represents the startup configuration for the event bus.
/// </summary>
public interface IEventBusStartup
{
    /// <summary>
    /// Initializes the event bus with the provided logger and event bus instance.
    /// </summary>
    /// <param name="logger">The logger instance for logging initialization details.</param>
    /// <param name="eventBus">The event bus instance to initialize.</param>
    public static abstract void Initialize(ILogger logger, IEventBus eventBus);

    /// <summary>
    /// Subscribes to integration events on the event bus with the provided logger and event bus instance.
    /// </summary>
    /// <param name="logger">The logger instance for logging subscription details.</param>
    /// <param name="eventBus">The event bus instance to subscribe to integration events.</param>
    public static abstract void SubscribeToIntegrationEvents(ILogger logger, IEventBus eventBus);
}
