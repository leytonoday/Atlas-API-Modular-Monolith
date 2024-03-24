namespace Atlas.Shared.Infrastructure.Persistance.Outbox;

public sealed class OutboxMessage
{
    /// <summary>
    /// The Id of the message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The full name of the message type.
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// The message data that has been serialised to JSON.
    /// </summary>
    public string Data { get; set; } = null!;

    /// <summary>
    /// The date and time that the event this outbox message stores occured.
    /// </summary>
    public DateTime OccurredOnUtc { get; set; }

    /// <summary>
    /// The date and time that this event this outbox message was processed successfully. This could mean either it was published successfully, or unsuccessfully. Either way, it was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; set; }

    /// <summary>
    /// The error thrown when attempting to consume the outbox message and publish it's <see cref="IDomainEvent"/>.
    /// </summary>
    public string? PublishError { get; set; }
}
