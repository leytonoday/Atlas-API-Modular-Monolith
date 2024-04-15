using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Shared.Application.Queue;

/// <summary>
/// Defines an interface for writing commands to a queue and managing their processing state.
/// </summary>
public interface IQueueWriter
{
    /// <summary>
    /// Writes a queued command asynchronously to the underlying queue system.
    /// </summary>
    /// <param name="queuedCommand">The queued command object to be written.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously checks if a queued command with the specified ID has already been processed by a particular handler.
    /// </summary>
    /// <param name="id">The unique identifier of the queued command.</param>
    /// <param name="handlerName">The name of the handler that might have processed the command.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that returns true if the command has been handled by the specified handler, false otherwise.</returns>
    Task<bool> IsQueueItemAlreadyHandledAsync(Guid id, string handlerName, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a queued command identified by its ID as handled by a specific handler.
    /// </summary>
    /// <param name="id">The unique identifier of the queued command.</param>
    /// <param name="handlerName">The name of the handler that processed the command.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <remarks>
    /// This method is typically called after successful processing of a queued command to prevent duplicate processing.
    /// </remarks>
    void MarkQueueItemAsHandled(Guid id, string handlerName, CancellationToken cancellationToken);
}
