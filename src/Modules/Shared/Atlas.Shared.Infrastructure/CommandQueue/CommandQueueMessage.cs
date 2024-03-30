using Atlas.Shared.Infrastructure.Integration.Outbox;
using Atlas.Shared.Infrastructure.Integration;
using Newtonsoft.Json;

namespace Atlas.Shared.Infrastructure.CommandQueue;

public class CommandQueueMessage
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
    /// The date and time that the event this command queue message stores occured.
    /// </summary>
    public DateTime OccurredOnUtc { get; private set; }

    /// <summary>
    /// The date and time that this event this command queue message was processed successfully. This could mean either it was published successfully, or unsuccessfully. Either way, it was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// The error thrown when attempting to consume the command queue message and publish it's <see cref="IQueuedCommand"/>.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Marks the <see cref="OutboxMessage"/> as processed, indicating that the <see cref="IIntegrationEvent"/> that it stores has been successfully published.
    /// </summary>
    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow; // TODO - Get date from IDateTimeProvider
    }

    /// <summary>
    /// Creates an <see cref="CommandQueueMessage"/> from an <see cref="IIntegrationEvent"/>
    /// </summary>
    /// <typeparam name="TCommand">The type of <see cref="IIntegrationEvent"/> to convert.</typeparam>
    /// <param name="command">The integration event to serialise and store as an <see cref="CommandQueueMessage"/>.</param>
    /// <returns>The newly created <see cref="CommandQueueMessage"/>.</returns>
    public static CommandQueueMessage CreateFromIntegrationEvent<TCommand>(TCommand command) where TCommand : IQueuedCommand
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Type = command.GetType().Name,
            OccurredOnUtc = DateTime.UtcNow,
            Data = JsonConvert.SerializeObject(
                command,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
        };
    }

    /// <summary>
    /// Deserialises an <see cref="CommandQueueMessage"/> into an <see cref="IQueuedCommand"/>.
    /// </summary>
    /// <param name="commandQueueMessage">The <see cref="CommandQueueMessage"/> to convert.</param>
    /// <returns>The deserialised <see cref="IQueuedCommand"/>.</returns>
    public static IQueuedCommand? ToRequest(CommandQueueMessage commandQueueMessage)
    {
        return JsonConvert
            .DeserializeObject<IQueuedCommand>(
                commandQueueMessage.Data,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
    }
}
