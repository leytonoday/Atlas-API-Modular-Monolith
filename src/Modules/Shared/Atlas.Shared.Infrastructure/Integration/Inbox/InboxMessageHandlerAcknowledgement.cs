namespace Atlas.Shared.Infrastructure.Integration.Inbox;

/// <summary>
/// Represents an acknowledgement of a successful handling of a <see cref="InboxMessage"/> by a particular handler.
/// </summary>
/// <remarks>
/// There is a unique constraint upon the InboxMessageId and HandlerName properties, meaning that a consumer (inbox message handler) has a record of processing any inbox message, and therefore
/// it won't be processed again by the same handler. This, in conjunction with the Inbox pattern, helps to enforce an exactly-once delivery (or processing) of inbox messages.
/// </remarks>
public class InboxMessageHandlerAcknowledgement
{
    /// <summary>
    /// The Id of the <see cref="InboxMessage"/> that was serialised and stored inside the <see cref="InboxMessage"/>.
    /// </summary>
    public Guid InboxMessageId { get; init; }

    /// <summary>
    /// The name of the event handler that has handled the <see cref="InboxMessage"/>
    /// </summary>
    public string HandlerName { get; init; } = null!;
}
