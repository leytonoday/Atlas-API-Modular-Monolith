namespace Atlas.Shared.Infrastructure.Integration.Outbox;

public interface IOutboxRepository
{
    public Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    public Task UpdateAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken);

    public Task<List<OutboxMessage>> ListPending(CancellationToken token);
}
