using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Shared.Application.Queue;

public interface IQueueWriter
{
    public Task WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken);

    public Task<bool> IsQueueItemAlreadyHandledAsync(Guid id, string handlerName, CancellationToken cancellationToken);

    public void MarkQueueItemAsHandled(Guid id, string handerName, CancellationToken cancellationToken);
}
