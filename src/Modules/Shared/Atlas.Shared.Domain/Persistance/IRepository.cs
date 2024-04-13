using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;
using System.Linq.Expressions;

namespace Atlas.Shared.Domain.Persistance;

/// <summary>
/// Represents a repository for working with entities of type <typeparamref name="TEntity"/> that don't have a single primary key.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    /// <summary>
    /// Gets all entities of type <typeparamref name="TEntity"/> from the database.
    /// </summary>
    /// <param name="trackChanges">Determines whether to track changes for the retrieved entities.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning the collection of entities.</returns>
    public Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken);

    /// <summary>
    /// Gets entities that satify the specifies <paramref name="condition"/> from the database.
    /// </summary>
    /// <param name="condition">The condition by which to filter the entities.</param>
    /// <param name="trackChanges">Determines whether to track changes for the retrieved entities.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning the collection of entities.</returns>
    public Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, bool trackChanges, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new entity of type <typeparamref name="TEntity"/> to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an existing entity of type <typeparamref name="TEntity"/> from the database.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="TEntity"/> in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the <see cref="IQueryable{TEntity}"/> for the <see cref="DbSet{TEntity}"/> of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="trackChanges">Determines whether to track changes for the retrieved entities.</param>
    /// <returns>The <see cref="IQueryable{TEntity}"/> for the <see cref="DbSet{TEntity}"/> of type <typeparamref name="TEntity"/>.</returns>
    /// <remarks>This can be used to query from the entire database table of type <see cref="TEntity"/>.</remarks>
    public IQueryable<TEntity> GetDbSet(bool trackChanges);

    /// <summary>
    /// Gets the <see cref="IQueryable{T}"/> for the <see cref="DbSet{T}"/> of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of entity to read from the database.</typeparam>
    /// <param name="trackChanges">Determines whether to track changes for the retrieved entities.</param>
    /// <returns>The <see cref="IQueryable{TEntity}"/> for the <see cref="DbSet{TEntity}"/> of type <typeparamref name="TEntity"/>.</returns>
    /// <remarks>This is different from <see cref="GetDbSet(bool)"/> because you can specify an entity other than the <typeparamref name="TEntity"/>.
    /// This may be used in cases where the <typeparamref name="TEntity"/> is an aggregate root and needs to load other dependant entities.</remarks>
    public IQueryable<T> GetDbSet<T>(bool trackChanges) where T : class, IEntity;
}

/// <summary>
/// Represents a repository for working with entities of type <typeparamref name="TEntity"/> that have a single primary key of type <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, in TId> : IRepository<TEntity> where TEntity : class, IAggregateRoot, IEntity<TId> 
{
    /// <summary>
    /// Gets an entity of type <typeparamref name="TEntity"/> from the database by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity.</param>
    /// <param name="trackChanges">Determins whether to track changes for the retrieved entity.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning the entity.</returns>
    public Task<TEntity?> GetByIdAsync(TId id, bool trackChanges, CancellationToken cancellationToken);
}
