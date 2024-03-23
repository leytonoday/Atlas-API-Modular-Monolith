namespace Atlas.Shared.Infrastructure.Persistance.Outbox;

/// <summary>
/// Represents an acknowledgement of a successful consuming of an outbox message domain event by a particular event handler.
/// </summary>
/// <remarks>
/// There is a unique constraint upon the DomainEventId and EventHandlerName properties, meaning that a consumer (domain event handler) has a record of processing any event, and therefore
/// it won't be processed again by the same handler. This, in conjunction with the Outbox Pattern enforces an exactly-once delivery guarantee of events.
/// </remarks>
public sealed class OutboxMessageConsumerAcknowledgement
{
    /// <summary>
    /// The Id of the <see cref="IDomainEvent"/> being that was serialised and stored inside the <see cref="OutboxMessage"/>.
    /// </summary>
    public Guid DomainEventId { get; init; }

    /// <summary>
    /// The name of the event handler that has consumed this event.
    /// </summary>
    public string EventHandlerName { get; init; } = null!;
}
