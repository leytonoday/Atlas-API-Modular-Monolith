using Atlas.Shared.Infrastructure.Integration.Outbox;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

public interface IInboxRepository
{
    public Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    public Task UpdateAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken);

    public Task<List<OutboxMessage>> ListPending(CancellationToken token);
}
