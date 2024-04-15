using Atlas.Shared.Application.Queue;

namespace Atlas.Shared.Infrastructure.Queue;

/// <summary>
/// Represents a contract for reading messages from a queue.
/// </summary>
public interface IQueueReader
{
    /// <summary>
    /// Retrieves a list of pending messages from the queue asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of pending <see cref="QueueMessage"/>.</returns>
    public Task<List<QueueMessage>> ListPendingAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specified queue message as processed asynchronously.
    /// </summary>
    /// <param name="commandQueueMessage">The queue message to mark as processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task MarkProcessedAsync(QueueMessage commandQueueMessage, CancellationToken cancellationToken);
}
