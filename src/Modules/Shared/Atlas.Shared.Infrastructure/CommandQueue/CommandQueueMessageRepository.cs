using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

public class CommandQueueMessageRepository(DbContext databaseContext) : 
    ICommandQueueMessageRepository
{
    private DbSet<CommandQueueMessage> GetDbSet() => databaseContext.Set<CommandQueueMessage>();

    public Task CreateAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(commandQueueMessage);

        return Task.CompletedTask;
    }

    public async Task<List<CommandQueueMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<CommandQueueMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .ToListAsync();
    }

    public Task MarkProcessedAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        commandQueueMessage.MarkProcessed();
        query.Update(commandQueueMessage);

        return Task.CompletedTask;
    }
}
