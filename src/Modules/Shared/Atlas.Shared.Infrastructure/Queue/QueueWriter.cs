using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

/// <inheritdoc cref="IQueueWriter"/>
public class QueueWriter<TDatabaseContext>(TDatabaseContext databaseContext) : IQueueWriter where TDatabaseContext : DbContext
{
    private DbSet<QueueMessage> GetDbSet() => databaseContext.Set<QueueMessage>();

    /// <inheritdoc/>
    public Task WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(QueueMessage.CreateFrom(queuedCommand));

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> IsQueueItemAlreadyHandledAsync(Guid id, string handlerName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await databaseContext.Set<QueueMessageHandlerAcknowledgement>()
            .AsNoTracking()
            .AnyAsync(x => x.QueuedCommandId == id && x.HandlerName == handlerName, cancellationToken);
    }

    /// <inheritdoc/>
    public void MarkQueueItemAsHandled(Guid id, string handerName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        databaseContext.Set<QueueMessageHandlerAcknowledgement>()
            .Add(new()
            {
                QueuedCommandId = id,
                HandlerName = handerName,
            });
    }
}
