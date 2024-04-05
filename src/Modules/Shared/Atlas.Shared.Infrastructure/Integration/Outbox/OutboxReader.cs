using Atlas.Shared.Infrastructure.Integration.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Queue;

public class OutboxReader<TDatabaseContext>(TDatabaseContext databaseContext) : IOutboxReader where TDatabaseContext : DbContext
{
    private DbSet<OutboxMessage> GetDbSet() => databaseContext.Set<OutboxMessage>();

    public async Task<List<OutboxMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }

    public Task MarkProcessedAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        outboxMessage.MarkProcessed();
        query.Update(outboxMessage);

        return Task.CompletedTask;
    }
}
