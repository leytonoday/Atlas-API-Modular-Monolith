using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration;

/// <summary>
/// Defines a contract for event handlers that process integration events published through an event bus.
/// </summary>
public interface IEventBusIntegrationEventHandler
{
    /// <summary>
    /// Asynchronously handles an integration event received from the event bus.
    /// </summary>
    /// <param name="event">The integration event object to be processed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Handle(IIntegrationEvent @event);
}
