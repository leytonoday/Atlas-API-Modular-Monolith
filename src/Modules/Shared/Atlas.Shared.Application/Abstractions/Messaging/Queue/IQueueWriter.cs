using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Shared.Application.Queue;

public interface IQueueWriter
{
    public Task WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken);
}
