namespace Atlas.Shared.Infrastructure.CommandQueue;

public interface ICommandQueueMessageRepository
{
    public Task CreateAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken);

    public Task MarkProcessedAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken);

    public Task<List<CommandQueueMessage>> ListPendingAsync(CancellationToken cancellationToken);
}
