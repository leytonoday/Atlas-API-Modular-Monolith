namespace Atlas.Shared.Infrastructure.Integration.Outbox;

/// <summary>
/// Represents a contract for reading pending messages from an outbox.
/// </summary>
public interface IOutboxReader
{
    /// <summary>
    /// Retrieves a list of pending messages from the outbox asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of pending <see cref="OutboxMessage"/>.</returns>
    Task<List<OutboxMessage>> ListPendingAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specified outbox message as processed asynchronously.
    /// </summary>
    /// <param name="ouboxMessage">The outbox message to mark as processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task MarkProcessedAsync(OutboxMessage ouboxMessage, CancellationToken cancellationToken);
}
