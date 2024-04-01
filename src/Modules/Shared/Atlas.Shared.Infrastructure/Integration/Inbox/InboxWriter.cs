using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

internal class InboxWriter<TDatabaseContext>(TDatabaseContext databaseContext) : IInboxWriter where TDatabaseContext : DbContext
{
    private DbSet<InboxMessage> GetDbSet() => databaseContext.Set<InboxMessage>();

    public Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(InboxMessage.CreateFromIntegrationEvent(integrationEvent));

        return Task.CompletedTask;
    }
}
