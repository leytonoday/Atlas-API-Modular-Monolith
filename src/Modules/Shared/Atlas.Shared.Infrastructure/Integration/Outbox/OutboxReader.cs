using Atlas.Shared.Infrastructure.Integration.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Queue;

/// <summary>
/// Implements the <see cref="IOutboxReader"/> interface for reading outbox messages from a DbContext of type <see cref="TDatabaseContext"/>.
/// </summary>
public class OutboxReader<TDatabaseContext>(TDatabaseContext databaseContext) : IOutboxReader where TDatabaseContext : DbContext
{
    private DbSet<OutboxMessage> GetDbSet() => databaseContext.Set<OutboxMessage>();

    /// <inheritdoc/>
    public async Task<List<OutboxMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null && x.PublishError == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task MarkProcessedAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        outboxMessage.MarkProcessed();
        query.Update(outboxMessage);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task MarkFailedAsync(OutboxMessage ouboxMessage, string errorMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        ouboxMessage.SetPublishError(errorMessage);
        query.Update(ouboxMessage);

        return Task.CompletedTask;
    }
}
