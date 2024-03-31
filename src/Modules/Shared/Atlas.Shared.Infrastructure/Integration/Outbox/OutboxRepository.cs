using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Outbox;

public class OutboxRepository(DbContext databaseContext) :
    IOutboxRepository
{
    private DbSet<OutboxMessage> GetDbSet() => databaseContext.Set<OutboxMessage>();


    public Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        var outboxMessage = OutboxMessage.CreateFromIntegrationEvent(integrationEvent);

        set.Add(outboxMessage);

        return Task.CompletedTask;
    }

    public async Task<List<OutboxMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync();
    }

    public Task MarkProcessedAsync(OutboxMessage outboxMessag, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        outboxMessag.MarkProcessed();
        query.Update(outboxMessag);

        return Task.CompletedTask;
    }
}
