using Atlas.Shared.Domain.Events;
using Atlas.Shared.Infrastructure.Persistance.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Persistance.Idempotance;

/// <summary>
/// Decorator for ensuring idempotence in handling application events by checking and recording their processed status.
/// </summary>
/// <typeparam name="TDomainEvent">Type of the domain event to be handled.</typeparam>
/// <typeparam name="DatabaseContext">Type of database context to be used in the decorator.</typeparam>
public sealed class DomainEventHandlerIdempotenceDecorator<TDomainEvent, DatabaseContext> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
    where DatabaseContext : DbContext, IDatabaseContext
{
    private readonly INotificationHandler<TDomainEvent> _decoratedHandler;
    private readonly DbContext _dbContext;

    public DomainEventHandlerIdempotenceDecorator(INotificationHandler<TDomainEvent> decoratedHandler, in DbContext dbContext)
    {
        _dbContext = dbContext;
        _decoratedHandler = decoratedHandler;
    }
    /// <summary>
    /// Handles the specified domain event with idempotence, ensuring it is processed only once.
    /// </summary>
    /// <param name="DomainEvent">The domain event to be handled.</param>
    /// <param name="cancellationToken">Propagates notifications that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(TDomainEvent DomainEvent, CancellationToken cancellationToken)
    {
        string eventHandlerName = _decoratedHandler.GetType().Name;

        // Check if this event has been handled by this event handler previously
        bool alreadyConsumedEvent = await _dbContext
            .Set<OutboxMessageConsumerAcknowledgement>()
            .AsNoTracking()
            .AnyAsync(x => x.DomainEventId == DomainEvent.Id && x.EventHandlerName == eventHandlerName, cancellationToken);

        if (alreadyConsumedEvent)
        {
            return;
        }

        // This is the actual event handler
        await _decoratedHandler.Handle(DomainEvent, cancellationToken);

        // If we get to this point, then we can assume an exception hasn't been thrown, and that the
        // event handler has executed successfully.

        // Create an entry to indicate that this domain event has been processed by this event handler
        _dbContext.Set<OutboxMessageConsumerAcknowledgement>()
            .Add(new() 
            {
                DomainEventId = DomainEvent.Id,
                EventHandlerName = eventHandlerName,
            });

        // Persist to database
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
