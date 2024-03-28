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
