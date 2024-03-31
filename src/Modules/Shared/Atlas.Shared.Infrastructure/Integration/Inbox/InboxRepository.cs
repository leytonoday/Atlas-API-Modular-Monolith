using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

public class InboxRepository(DbContext databaseContext) :
    IInboxRepository
{
    private DbSet<InboxMessage> GetDbSet() => databaseContext.Set<InboxMessage>();


    public Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        var outboxMessage = InboxMessage.CreateFromIntegrationEvent(integrationEvent);

        set.Add(outboxMessage);

        return Task.CompletedTask;
    }

    public async Task<List<InboxMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<InboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync();
    }

    public Task MarkProcessedAsync(InboxMessage inboxMessag, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        inboxMessag.MarkProcessed();
        query.Update(inboxMessag);

        return Task.CompletedTask;
    }
}
