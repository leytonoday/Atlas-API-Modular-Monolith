namespace Atlas.Shared.Domain.Entities;

/// <summary>
/// Represents an entity that doesn't have a single primary key.
/// </summary>
public interface IEntity;

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