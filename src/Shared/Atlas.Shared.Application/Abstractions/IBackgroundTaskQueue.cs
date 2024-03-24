namespace Atlas.Shared.Application.Abstractions;

/// <summary>
/// Represents a queue of tasks to be executed in the background of the application.
/// </summary>
public interface IBackgroundTaskQueue
{
    /// <summary>
    /// Enqueues a new task item in the queue, that will be dequeued and processed by some service shortly after.
    /// </summary>
    /// <param name="task">The delegate task item to enqueue.</param>
    /// <returns>A <see cref="ValueTask"/> that returns when the enqueuing of the <paramref name="task"/> is complete.</returns>
    public ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> task);

    /// <summary>
    /// Dequeues a task item from the queue.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the dequeue operation.</param>
    /// <returns>A <see cref="ValueTask{Func{CancellationToken, ValueTask}}"/> representing the task item that was dequeued.</returns>
    /// <remarks>If the queue is empty, this method will wait until something is added to the queue before returning.</remarks>
    public ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the number of jobs currently in the background task queue.
    /// </summary>
    /// <returns>A positive integer that represents the amount of jobs in the queue.</returns>
    public uint GetJobCount();
}
