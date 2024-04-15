using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

/// <inheritdoc cref="IInboxWriter"/>
internal class InboxWriter<TDatabaseContext>(TDatabaseContext databaseContext) : IInboxWriter where TDatabaseContext : DbContext
{
    private DbSet<InboxMessage> GetDbSet() => databaseContext.Set<InboxMessage>();

    /// <inheritdoc/>
    public Task WriteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(InboxMessage.CreateFromIntegrationEvent(integrationEvent));

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> IsInboxItemAlreadyHandledAsync(Guid id, string handlerName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await databaseContext.Set<InboxMessageHandlerAcknowledgement>()
            .AsNoTracking()
            .AnyAsync(x => x.InboxMessageId == id && x.HandlerName == handlerName, cancellationToken);
    }

    /// <inheritdoc/>
    public void MarkInboxItemAsHandled(Guid id, string handerName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        databaseContext.Set<InboxMessageHandlerAcknowledgement>()
            .Add(new()
            {
                InboxMessageId = id,
                HandlerName = handerName,
            });
    }
}
