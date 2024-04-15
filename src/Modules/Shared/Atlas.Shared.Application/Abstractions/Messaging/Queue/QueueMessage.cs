using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Newtonsoft.Json;

namespace Atlas.Shared.Application.Queue;

/// <summary>
/// Represents a message stored in a queue system. 
/// This message contains information about an event and the data associated with it.
/// </summary>
public class QueueMessage
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
    /// The date and time that the event this queue message stores occured.
    /// </summary>
    public DateTime OccurredOnUtc { get; private set; }

    /// <summary>
    /// The date and time that this event this queue message was processed successfully. This could mean either it was published successfully, or unsuccessfully. Either way, it was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// The error thrown when attempting to consume the queue message and publish it's <see cref="QueuedCommand"/>.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Marks the <see cref="QueueMessage"/> as processed, indicating that the <see cref="QueuedCommand"/> that it stores has been successfully execited.
    /// </summary>
    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow; // TODO - Get date from IDateTimeProvider
    }

    /// <summary>
    /// Sets the error value on the <see cref="QueueMessage"/> indicating that this message could not be published for whatever reason. Think of setting this column to anything other than NULL the same as 
    /// moving the message to a dead-letter-queue.
    /// </summary>
    /// <param name="error">The error message from the <see cref="Exception"/> that was thrown when attempting to run the command.</param>
    public void SetError(string error)
    {
        Error = error;
    }

    public static QueueMessage CreateFrom<TCommand>(TCommand command) where TCommand : QueuedCommand
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
    /// Deserialises an <see cref="QueueMessage"/> into an <see cref="QueuedCommand"/>.
    /// </summary>
    /// <param name="commandQueueMessage">The <see cref="QueueMessage"/> to convert.</param>
    /// <returns>The deserialised <see cref="QueuedCommand"/>.</returns>
    public static QueuedCommand? ToRequest(QueueMessage commandQueueMessage)
    {
        return JsonConvert
            .DeserializeObject<QueuedCommand>(
                commandQueueMessage.Data,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
    }
}
