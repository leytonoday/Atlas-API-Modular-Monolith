using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration.Inbox;

public interface IInboxWriter
{
    public Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    public Task<bool> IsInboxItemAlreadyHandledAsync(Guid id, string handlerName, CancellationToken cancellationToken);

    public void MarkInboxItemAsHandled(Guid id, string handerName, CancellationToken cancellationToken);
}
