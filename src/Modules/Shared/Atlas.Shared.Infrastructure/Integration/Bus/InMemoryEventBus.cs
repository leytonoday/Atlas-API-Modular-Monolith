using Atlas.Shared.Application.Abstractions.Integration;
using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Infrastructure.Integration.Bus;

/// <summary>
/// Represents an in-memory event bus implementation that facilitates communication between different parts of the application through events.
/// </summary>
public class InMemoryEventBus : IEventBus
{
    /// <summary>
    /// Dictionary storing event subscriptions where the key is the event type name and the value is a list of event handlers.
    /// </summary>
    private readonly Dictionary<string, List<IEventBusIntegrationEventHandler>> _subscriptions = [];

    /// <inheritdoc />
    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IIntegrationEvent
    {
        string eventTypeName = GetEventTypeName(@event);

        // Get all event handlers that have previously subscribed to this event type
        List<IEventBusIntegrationEventHandler> eventHandlers = GetEventHandlers(eventTypeName);
        if (eventHandlers.Count == 0)
        {
            return;
        }

        // Execute all event handlers
        foreach (IEventBusIntegrationEventHandler eventHandler in eventHandlers)
        {
            await eventHandler.Handle(@event);
        }
    }

    /// <inheritdoc />
    public void Subscribe<TEvent>(IEventBusIntegrationEventHandler eventHandler) where TEvent : IIntegrationEvent
    {
        string eventTypeName = GetEventTypeName<TEvent>();

        // If there isn't already a List in the subscriptions dictionary for this type of event, then add it, along with the new event handler
        if (!_subscriptions.TryGetValue(eventTypeName, out var handlers))
        {
            _subscriptions.Add(eventTypeName, [eventHandler]);
        }
        else
        {
            handlers.Add(eventHandler);
        }
    }

    /// <summary>
    /// Retrieves the full type name of the specified event type.
    /// </summary>
    /// <typeparam name="T">Type of the event.</typeparam>
    /// <returns>The full type name of the event.</returns>
    private static string GetEventTypeName<T>() where T : IIntegrationEvent =>
        typeof(T).FullName!;

    /// <summary>
    /// Retrieves the full type name of the specified event object.
    /// </summary>
    /// <typeparam name="T">Type of the event object.</typeparam>
    /// <param name="event">The event object.</param>
    /// <returns>The full type name of the event object.</returns>
    private static string GetEventTypeName<T>(T @event) where T : IIntegrationEvent =>
        @event.GetType().FullName!;

    /// <summary>
    /// Retrieves the list of event handlers subscribed to the specified event type.
    /// </summary>
    /// <param name="key">The event type name.</param>
    /// <returns>The list of event handlers.</returns>
    private List<IEventBusIntegrationEventHandler> GetEventHandlers(string key) =>
        _subscriptions.TryGetValue(key, out var handlers)
            ? handlers
            : new List<IEventBusIntegrationEventHandler>();
}
