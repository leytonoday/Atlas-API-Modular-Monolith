using Atlas.Shared.Application.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Queue;

public class QueueReader(DbContext databaseContext)
{
    private DbSet<QueueMessage> GetDbSet() => databaseContext.Set<QueueMessage>();

    public async Task<List<QueueMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<QueueMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }

    public Task MarkProcessedAsync(QueueMessage commandQueueMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        commandQueueMessage.MarkProcessed();
        query.Update(commandQueueMessage);

        return Task.CompletedTask;
    }
}
