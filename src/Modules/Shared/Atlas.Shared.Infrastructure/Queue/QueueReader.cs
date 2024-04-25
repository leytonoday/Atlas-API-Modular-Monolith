using Atlas.Shared.Application.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Queue;

/// <inheritdoc cref="IQueueReader"/>
public class QueueReader<TDatabaseContext>(TDatabaseContext databaseContext) : IQueueReader where TDatabaseContext : DbContext
{
    private DbSet<QueueMessage> GetDbSet() => databaseContext.Set<QueueMessage>();

    /// <inheritdoc/>
    public async Task<List<QueueMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<QueueMessage>()
            .Where(x => x.ProcessedOnUtc == null && x.Error == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task MarkProcessedAsync(QueueMessage commandQueueMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        commandQueueMessage.MarkProcessed();
        query.Update(commandQueueMessage);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task MarkFailedAsync(QueueMessage commandQueueMessage, string errorMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        commandQueueMessage.SetError(errorMessage);
        query.Update(commandQueueMessage);

        return Task.CompletedTask;
    }
}
