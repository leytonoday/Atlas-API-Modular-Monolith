using Atlas.Shared.Application.Queue;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

public interface IInboxReader
{
    public Task<List<InboxMessage>> ListPendingAsync(CancellationToken cancellationToken);

    public Task MarkProcessedAsync(InboxMessage inboxMessage, CancellationToken cancellationToken);
}