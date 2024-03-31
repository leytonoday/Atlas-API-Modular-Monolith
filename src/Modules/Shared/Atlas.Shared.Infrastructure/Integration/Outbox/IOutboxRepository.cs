namespace Atlas.Shared.Infrastructure.Integration.Outbox;

public interface IOutboxRepository
{
    public Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    public Task MarkProcessedAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken);

    public Task<List<OutboxMessage>> ListPendingAsync(CancellationToken cancellationToken);
}
