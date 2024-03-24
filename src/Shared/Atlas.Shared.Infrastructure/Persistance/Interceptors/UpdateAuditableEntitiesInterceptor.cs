using Atlas.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Atlas.Infrastructure.Persistance.Interceptors;

/// <summary>
/// Interceptor for updating audit-related properties on entities implementing the <see cref="IAuditableEntity"/> interface
/// before changes are saved to the database.
/// </summary>
public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts the process of saving changes to the database and updates audit-related properties on entities.
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

        // Get all entities that implement the IAuditableEntity interface
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            // If the entity is new, set the created date and time
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            }

            // If the entity has been updated, set the updated date and time
            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.UpdatedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }
}
