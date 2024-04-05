namespace Atlas.Shared.Infrastructure.Queue;

/// <summary>
/// Represents an acknowledgement of a successful handling of a <see cref="QueueMessage"/> by a particular handler.
/// </summary>
/// <remarks>
/// There is a unique constraint upon the QueuedCommandId and HandlerName properties, meaning that a consumer (command event handler) has a record of processing any command, and therefore
/// it won't be processed again by the same handler. This, in conjunction with the Command Queue pattern, helps to enforce an exactly-once delivery guarantee of commands.
/// </remarks>
public class QueueMessageHandlerAcknowledgement
{
    /// <summary>
    /// The Id of the <see cref="QueuedCommand"/> that was serialised and stored inside the <see cref="QueueMessage"/>.
    /// </summary>
    public Guid QueuedCommandId { get; init; }

    /// <summary>
    /// The name of the event handler that has handled the <see cref="QueuedCommand"/>
    /// </summary>
    public string HandlerName { get; init; } = null!;
}
