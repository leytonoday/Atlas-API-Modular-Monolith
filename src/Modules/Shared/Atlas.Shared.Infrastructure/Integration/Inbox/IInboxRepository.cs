using Atlas.Shared.Infrastructure.CommandQueue;
using Atlas.Shared.Infrastructure.Integration.Outbox;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

public interface IInboxRepository
{
    public Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    public Task MarkProcessedAsync(InboxMessage inboxMessage, CancellationToken cancellationToken);

    public Task<List<InboxMessage>> ListPendingAsync(CancellationToken cancellationToken);
}
