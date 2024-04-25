namespace Atlas.Shared.Infrastructure.Integration.Inbox;

/// <summary>
/// Defines an interface for reading messages from an inbox.
/// </summary>
public interface IInboxReader
{
    /// <summary>
    /// Retrieves a list of pending messages from the inbox.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that returns a list of <see cref="InboxMessage"/> objects.</returns>
    Task<List<InboxMessage>> ListPendingAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specific inbox message as processed.
    /// </summary>
    /// <param name="inboxMessage">The inbox message to mark as processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An awaitable task.</returns>
    Task MarkProcessedAsync(InboxMessage inboxMessage, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specified inbox message as failed asynchronously.
    /// </summary>
    /// <param name="inboxMessage">The inbox message to mark as failed.</param>
    /// <param name="errorMessage">The error that caused the inbox message to fail publishing.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task MarkFailedAsync(InboxMessage inboxMessage, string errorMessage, CancellationToken cancellationToken);
}
