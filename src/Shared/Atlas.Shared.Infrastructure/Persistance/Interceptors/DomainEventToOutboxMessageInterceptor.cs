using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Events;
using Atlas.Shared.Infrastructure.Persistance.Outbox;

namespace Atlas.Infrastructure.Persistance.Interceptors;

/// <summary>
/// Interceptor for saving all application events as <see cref="OutboxMessage"/> entities within the database.
/// </summary>
internal sealed class DomainEventToOutboxMessageInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts the process of saving changes to the database and converts all <see cref="IDomainEvent"/> instances to <see cref="OutboxMessage"/>s.
    /// </summary>
    /// <param name="eventData">The event data for the context of saving changes.</param>
    /// <param name="result">The result of the interception.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> representing the asynchronous interception result.
    /// </returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }

        // Extract application events from all aggregate roots within the change tracker.
        IEnumerable<IDomainEvent> domainEvents = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot => aggregateRoot.DomainEvents);

        // Convert application events to outbox messages
        List<OutboxMessage> outboxMessages = domainEvents
            .Select(domainEvent => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                // Using Newtonsoft.Json because of a lack of support for polymorphic deserialisation within System.Text.Json 
                Data = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        // Add all application events at once to the database
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(
            eventData, 
            result, 
            cancellationToken);
    }
}
