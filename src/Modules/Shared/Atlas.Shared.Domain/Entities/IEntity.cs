using Atlas.Shared.Domain.BusinessRules;
using Atlas.Shared.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Shared.Domain.Entities;

/// <summary>
/// Represents an entity that doesn't have a single primary key.
/// </summary>
public interface IEntity
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
    void ClearDomainEvents();
}

/// <summary>
/// Represents an entity that has a single primary key.
/// </summary>
/// <typeparam name="TId">The type of the primary key of this entity.</typeparam>
public interface IEntity<TId> : IEntity
{
    /// <summary>
    /// Gets the Id of the <see cref="IEntity{TId}"/>.
    /// </summary>
    public TId Id { get; }
}