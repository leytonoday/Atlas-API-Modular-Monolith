using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration.Outbox;

public interface IOutboxWriter
{
    public Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
