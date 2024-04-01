namespace Atlas.Shared.Infrastructure.Integration.Outbox;

public interface IOutboxReader
{
    public Task<List<OutboxMessage>> ListPendingAsync(CancellationToken cancellationToken);

    public Task MarkProcessedAsync(OutboxMessage ouboxMessage, CancellationToken cancellationToken);
}