using Atlas.Users.Domain;
using Atlas.Users.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Users.Infrastructure.Persistance;

internal sealed class UsersUnitOfWork(UsersDatabaseContext usersDatabaseContext) : IUsersUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        using var dbContextTransaction = usersDatabaseContext.Database.BeginTransaction();

        try
        {
            _ = await usersDatabaseContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            //Log Exception Handling message                      
            await dbContextTransaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task<bool> HasDomainEventBeenHandledAsync(string eventHandlerName, Guid domainEventId, CancellationToken cancellationToken)
    {
        return await usersDatabaseContext
            .Set<UsersOutboxMessageConsumerAcknowledgement>()
            .AsNoTracking()
            .AnyAsync(x => x.DomainEventId == domainEventId && x.EventHandlerName == eventHandlerName, cancellationToken);
    }

    public void MarkDomainEventAsHandled(string eventHandlerName, Guid domainEventId, CancellationToken cancellationToken)
    {
        usersDatabaseContext.Set<UsersOutboxMessageConsumerAcknowledgement>()
            .Add(new UsersOutboxMessageConsumerAcknowledgement()
            {
                DomainEventId = domainEventId,
                EventHandlerName = eventHandlerName
            });
    }
}
