using Atlas.Shared.Infrastructure.Integration.Inbox;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Queue;

/// <summary>
/// Implements the <see cref="IInboxReader"/> interface for reading inbox messages from a DbContext of type <see cref="TDatabaseContext"/>.
/// </summary>
public class InboxReader<TDatabaseContext>(TDatabaseContext databaseContext) : IInboxReader where TDatabaseContext : DbContext
{
    private DbSet<InboxMessage> GetDbSet() => databaseContext.Set<InboxMessage>();

    /// <inheritdoc/>
    public async Task<List<InboxMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<InboxMessage>()
            .Where(x => x.ProcessedOnUtc == null && x.PublishError == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task MarkProcessedAsync(InboxMessage inboxMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        inboxMessage.MarkProcessed();
        query.Update(inboxMessage);

        return Task.CompletedTask;
    }
}
