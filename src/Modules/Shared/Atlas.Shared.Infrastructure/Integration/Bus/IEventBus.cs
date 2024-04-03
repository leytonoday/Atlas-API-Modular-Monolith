using Atlas.Shared.Application.Abstractions.Integration;
using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Infrastructure.Integration.Bus;

/// <summary>
/// Represents an event bus for facilitating communication between sub-domains within a Domain Driven Design (DDD) monolith or services within a microservices architecture.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an event to all registered event handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to be published.</typeparam>
    /// <param name="event">The event object to be published.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IIntegrationEvent;

    /// <summary>
    /// Subscribes an event handler to handle events of a specific type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to subscribe to.</typeparam>
    /// <param name="eventHandler">The event handler to be subscribed.</param>
    void Subscribe<TEvent>(IEventBusIntegrationEventHandler eventHandler) where TEvent : IIntegrationEvent;
}
