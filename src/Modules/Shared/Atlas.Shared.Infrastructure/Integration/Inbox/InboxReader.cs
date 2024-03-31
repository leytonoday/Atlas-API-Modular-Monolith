using Atlas.Shared.Infrastructure.Integration.Inbox;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Queue;

public class InboxReader(DbContext databaseContext)
{
    private DbSet<InboxMessage> GetDbSet() => databaseContext.Set<InboxMessage>();

    public async Task<List<InboxMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<InboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }

    public Task MarkProcessedAsync(InboxMessage inboxMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        inboxMessage.MarkProcessed();
        query.Update(inboxMessage);

        return Task.CompletedTask;
    }
}
