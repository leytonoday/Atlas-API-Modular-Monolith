using Atlas.Shared.Application.Queue;

namespace Atlas.Shared.Infrastructure.Queue;

public interface IQueueReader
{
    public Task<List<QueueMessage>> ListPendingAsync(CancellationToken cancellationToken);

    public Task MarkProcessedAsync(QueueMessage commandQueueMessage, CancellationToken cancellationToken);
}