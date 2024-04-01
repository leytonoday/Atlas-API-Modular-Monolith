using Atlas.Shared.Application.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

public class QueueWriter<TDatabaseContext>(TDatabaseContext databaseContext) : IQueueWriter where TDatabaseContext : DbContext
{
    private DbSet<QueueMessage> GetDbSet() => databaseContext.Set<QueueMessage>();

    public Task WriteAsync(IQueuedCommand queuedCommand, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(QueueMessage.CreateFrom(queuedCommand));

        return Task.CompletedTask;
    }
}
