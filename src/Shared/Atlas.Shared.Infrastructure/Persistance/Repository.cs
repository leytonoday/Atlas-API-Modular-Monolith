using System.Linq.Expressions;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Infrastructure.Persistance;

/// <inheritdoc cref="IRepository{TEntity}"/>
public abstract class Repository<TEntity, TDatabaseContext>
    : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDatabaseContext : DbContext
{
    protected readonly TDatabaseContext Context;
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>
    /// Initialises a new instance of the <see cref="Repository{TEntity}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    protected Repository(TDatabaseContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    /// <inheritdoc/>
    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // We know that the Add here isn't gonna be querying the
        // database to create the value for a column, so there's no blocking.
        // So we just use Add to avoid the overhead of AddAsync
        DbSet.Add(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetByConditionAsync(null, trackChanges, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>>? condition, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<TEntity> query = GetDbSet(trackChanges);
        if (condition != null)
            query = query.Where(condition);

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> GetDbSet(bool trackChanges)
    {
        return trackChanges ? DbSet : DbSet.AsNoTracking();
    }
}

/// <inheritdoc cref="IRepository{TEntity, TId}"/>
public abstract class Repository<TEntity, TDatabaseContext, TId> : Repository<TEntity, TDatabaseContext>, IRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TDatabaseContext : DbContext
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Repository{TEntity, TId}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    protected Repository(TDatabaseContext context) : base(context) { }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> GetByIdAsync(TId id, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<TEntity> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
}