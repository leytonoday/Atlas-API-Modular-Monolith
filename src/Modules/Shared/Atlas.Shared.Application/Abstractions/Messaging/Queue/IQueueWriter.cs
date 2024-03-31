namespace Atlas.Shared.Application.Queue;

public interface IQueueWriter
{
    public Task WriteAsync(IQueuedCommand queuedCommand, CancellationToken cancellationToken);
}
