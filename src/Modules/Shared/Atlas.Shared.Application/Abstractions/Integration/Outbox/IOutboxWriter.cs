using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration.Outbox;

/// <summary>
/// Defines a contract for writing integration events to an outbox.
/// </summary>
public interface IOutboxWriter
{
    /// <summary>
    /// Asynchronously writes an integration event to the underlying outbox system.
    /// </summary>
    /// <param name="integrationEvent">The integration event object to be written.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
