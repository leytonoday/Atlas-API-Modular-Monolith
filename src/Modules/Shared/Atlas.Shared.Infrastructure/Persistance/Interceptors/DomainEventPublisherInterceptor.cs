using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Atlas.Shared.Infrastructure.Persistance.Interceptors;

/// <summary>
/// Intercepts database context when saving changes, and publishes all domain events.
/// </summary>
/// <param name="publisher">The MediatR <see cref="IPublisher"/> used to publish domain events.</param>
public sealed class DomainEventPublisherInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return await base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }

        // Extract domain events from all aggregate roots within the change tracker.
        IEnumerable<IDomainEvent> domainEvents = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot => aggregateRoot.DomainEvents);

        // Iterate over each domain event and publish it sequentially
        foreach(IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }
}
