namespace Atlas.Shared.Infrastructure.CommandQueue;

public interface ICommandQueueMessageRepository
{
    public Task CreateAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken);

    public Task UpdateAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken);

    public Task<List<CommandQueueMessage>> ListPending(CancellationToken token);
}
