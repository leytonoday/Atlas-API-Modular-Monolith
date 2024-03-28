using Atlas.Shared.Application.Abstractions;
using System.Threading.Channels;

namespace Atlas.Shared.Infrastructure;


/// <inheritdoc />
internal sealed class BackgroundTaskQueue : IBackgroundTaskQueue
{
    /// <summary>
    /// Represents a queue of asynchronous operations, each encapsulated as a <see cref="Func{T, TResult}"/> that takes a <see cref="CancellationToken"/> 
    /// and returns a <see cref="ValueTask"/>. This queue is designed for managing and executing asynchronous tasks in a serialized fashion.
    /// </summary>
    /// <remarks>
    /// The <see cref="Channel{T}"/> is used to store and process the asynchronous operations in a first-in-first-out (FIFO) order. 
    /// Consumers can enqueue new operations by adding delegates to the channel, and a dedicated worker can dequeue and execute these 
    /// operations, ensuring proper synchronization and execution of asynchronous tasks.
    /// </remarks>
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    /// <summary>
    /// Represents the current count of jobs or tasks being managed. This count is used to track the number of active jobs in a concurrent environment.
    /// </summary>
    private uint _jobCount;

    /// <summary>
    /// A synchronization object used to provide exclusive access to the _jobCount property to prevent multiple threads from updating the value simulaneously. 
    /// This service is a singleton, so this must be considered.
    /// </summary>
    private readonly object _lockObj;

    /// <summary>
    /// Initialises a new instance of the <see cref="BackgroundTaskQueue"/> class.
    /// </summary>
    /// <param name="capacity">The maximum amount of tasks that can be in the queue.</param>
    public BackgroundTaskQueue(int capacity)
    {
        BoundedChannelOptions options = new(capacity)
        {
            // If the queue is full, attempts to enqueue to wait until a space is available.
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        _jobCount = 0;
        _lockObj = new object();
    }

    /// <inheritdoc />
    public async ValueTask EnqueueAsync(
        Func<CancellationToken, ValueTask> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        await _queue.Writer.WriteAsync(workItem);

        lock (_lockObj)
        {
            _jobCount++;
        }
    }

    /// <inheritdoc />
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        Func<CancellationToken, ValueTask>? workItem =
            await _queue.Reader.ReadAsync(cancellationToken);

        lock (_lockObj)
        {
            _jobCount--;
        }

        return workItem;
    }

    /// <inheritdoc />
    public uint GetJobCount()
    {
        return _jobCount;
    }
}