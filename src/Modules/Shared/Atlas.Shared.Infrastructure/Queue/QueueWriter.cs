using Atlas.Shared.Application.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

public class QueueWriter(DbContext databaseContext) : IQueueWriter
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
