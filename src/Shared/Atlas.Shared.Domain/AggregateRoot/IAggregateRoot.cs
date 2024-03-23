using Atlas.Shared.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Shared.Domain.AggregateRoot;

public interface IAggregateRoot
{
    /// <summary>
    /// Gets the collection of application events associated with the entity.
    /// </summary>
    [NotMapped]
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Adds a domain event to the collection, triggering a notification handler elsewhere for this domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    /// <remarks>A domain event should be raised when a command has a side-effect that must occur. For example, the sending of an email when the user registers.</remarks>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Removes a domain event from the collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to be removed.</param>
    void RemoveDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Clears all application events associated with the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to be cleared.</param>
    void ClearDomainEvent(IDomainEvent domainEvent);
}