using Atlas.Shared.Domain.BusinessRules;
using Atlas.Shared.Domain.Events;
using Atlas.Shared.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Shared.Domain.Entities;

/// <summary>
/// Represents a base class implementation of <see cref="IEntity"/>, for entities that don't have a single primary key.
/// </summary>
public abstract class Entity : IEntity, IAuditableEntity
{
    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; init; }

    /// <inheritdoc />
    public DateTime? UpdatedOnUtc { get; init; }

    private readonly List<IDomainEvent> _domainEvents = [];

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    /// <inheritdoc />
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <inheritdoc />
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    /// <inheritdoc />
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Ensures a given business rule has not been broken.
    /// </summary>
    /// <param name="rule">The business rule to ensure isn't broken.</param>
    /// <exception cref="BusinessRuleBrokenException" />
    protected static void CheckBusinessRule(IBusinessRule rule)
    {
        if (rule.IsBroken()) throw new BusinessRuleBrokenException(rule.Message, rule.Code);
    }

    /// <summary>
    /// Ensures a given asynchronous business rule has not been broken.
    /// </summary>
    /// <param name="rule">The business rule to ensure isn't broken.</param>
    /// <exception cref="BusinessRuleBrokenException" />
    protected static async Task CheckAsyncBusinessRule(IAsyncBusinessRule rule, CancellationToken cancellationToken = default)
    {
        if (await rule.IsBrokenAsync(cancellationToken)) throw new BusinessRuleBrokenException(rule.Message, rule.Code);
    }
}

/// <summary>
/// Represents a base class implementation of <see cref="IEntity{TId}"/>, for entities that have a single primary key.
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class Entity<TId> : Entity, IEntity<TId>
{
    /// <summary>
    /// Gets or sets the Id of the <see cref="Entity{TId}"/>.
    /// </summary>
    public virtual TId Id { get; set; } = default!;
}
