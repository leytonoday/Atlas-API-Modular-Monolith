using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration.Inbox;

/// <summary>
/// Defines a contract for writing integration events to an inbox and managing their processing state.
/// </summary>
public interface IInboxWriter
{
    /// <summary>
    /// Asynchronously writes an integration event to the underlying inbox system.
    /// </summary>
    /// <param name="integrationEvent">The integration event object to be written.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously checks if an inbox item with the specified ID has already been processed by a particular handler.
    /// </summary>
    /// <param name="id">The unique identifier of the inbox item.</param>
    /// <param name="handlerName">The name of the handler that might have processed the item.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that returns true if the item has been handled by the specified handler, false otherwise.</returns>
    Task<bool> IsInboxItemAlreadyHandledAsync(Guid id, string handlerName, CancellationToken cancellationToken);

    /// <summary>
    /// Marks an inbox item identified by its ID as handled by a specific handler.
    /// </summary>
    /// <param name="id">The unique identifier of the inbox item.</param>
    /// <param name="handlerName">The name of the handler that processed the item.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <remarks>
    /// This method is typically called after successful processing of an inbox item to prevent duplicate processing.
    /// </remarks>
    void MarkInboxItemAsHandled(Guid id, string handerName, CancellationToken cancellationToken);
}
