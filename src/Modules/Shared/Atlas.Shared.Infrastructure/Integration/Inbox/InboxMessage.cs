using Atlas.Shared.IntegrationEvents;
using Newtonsoft.Json;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

/// <summary>
/// Represents an inbox message that stores an integration event for later processing.
/// </summary>
public class InboxMessage
{
    /// <summary>
    /// The Id of the message.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The full name of the message type.
    /// </summary>
    public string Type { get; private set; } = null!;

    /// <summary>
    /// The message data that has been serialised to JSON.
    /// </summary>
    public string Data { get; private set; } = null!;

    /// <summary>
    /// The date and time that the event this inbox message stores occured.
    /// </summary>
    public DateTime OccurredOnUtc { get; private set; }

    /// <summary>
    /// The date and time that this event this inbox message was processed successfully. This could mean either it was published successfully, or unsuccessfully. Either way, it was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// The error thrown when attempting to consume the inbox message and publish it's <see cref="IIntegrationEvent"/>.
    /// </summary>
    public string? PublishError { get; private set; }

    /// <summary>
    /// Marks the <see cref="InboxMesage"/> as processed, indicating that the <see cref="IIntegrationEvent"/> that it stores has been successfully published.
    /// </summary>
    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow; // TODO - Get date from IDateTimeProvider
    }

    /// <summary>
    /// Sets the error value on the <see cref="InboxMessage"/> indicating that this message could not be published for whatever reason. Think of setting this column to anything other than NULL the same as 
    /// moving the message to a dead-letter-queue.
    /// </summary>
    /// <param name="error">The error message from the <see cref="Exception"/> that was thrown when attempting to run the integration event.</param>
    public void SetPublishError(string error)
    {
        PublishError = error;
    }

    /// <summary>
    /// Creates an <see cref="InboxMesage"/> from an <see cref="IIntegrationEvent"/>
    /// </summary>
    /// <typeparam name="TEvent">The type of <see cref="IIntegrationEvent"/> to convert.</typeparam>
    /// <param name="integrationEvent">The integration event to serialise and store as an <see cref="InboxMesage"/>.</param>
    /// <returns>The newly created <see cref="InboxMesage"/>.</returns>
    public static InboxMessage CreateFromIntegrationEvent<TEvent>(TEvent integrationEvent) where TEvent : IIntegrationEvent
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Type = integrationEvent.GetType().Name,
            OccurredOnUtc = DateTime.UtcNow,
            Data = JsonConvert.SerializeObject(
                integrationEvent,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
        };
    }

    /// <summary>
    /// Deserialises an <see cref="InboxMesage"/> into an <see cref="IIntegrationEvent"/>.
    /// </summary>
    /// <param name="InboxMesage">The <see cref="InboxMesage"/> to convert.</param>
    /// <returns>The deserialised <see cref="IIntegrationEvent"/>.</returns>
    public static IIntegrationEvent? ToIntegrationEvent(InboxMessage inboxMessage)
    {
        return JsonConvert
            .DeserializeObject<IIntegrationEvent>(
                inboxMessage.Data,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
    }
}
