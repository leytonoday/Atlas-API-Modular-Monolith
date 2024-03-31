using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration.Inbox;

public interface IInboxWriter
{
    public Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
