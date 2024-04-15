using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Outbox;

/// <inheritdoc cref="IOutboxWriter"/>
internal class OutboxWriter<TDatabaseContext>(TDatabaseContext databaseContext) : IOutboxWriter where TDatabaseContext : DbContext
{
    private DbSet<OutboxMessage> GetDbSet() => databaseContext.Set<OutboxMessage>();

    /// <inheritdoc/>
    public Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(OutboxMessage.CreateFromIntegrationEvent(integrationEvent));

        return Task.CompletedTask;
    }
}
