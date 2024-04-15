using Atlas.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Shared.Infrastructure.Persistance;

/// <inheritdoc cref="IUnitOfWork"/>
public abstract class BaseUnitOfWork<TDatabaseContext, IUnitOfWorkLogger>(TDatabaseContext databaseContext, IUnitOfWorkLogger logger) : IUnitOfWork 
    where TDatabaseContext : DbContext
    where IUnitOfWorkLogger : ILogger
{ 
    /// <inheritdoc/>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        using var dbContextTransaction = databaseContext.Database.BeginTransaction();
        logger.LogDebug("Started transaction - {TransactionId}", dbContextTransaction.TransactionId);

        try
        {
            logger.LogDebug("Saving changes - {TransactionId}", dbContextTransaction.TransactionId);
            _ = await databaseContext.SaveChangesAsync(cancellationToken);

            await dbContextTransaction.CommitAsync(cancellationToken);
            logger.LogDebug("Commiting transaction - {TransactionId}", dbContextTransaction.TransactionId);
        }
        catch (Exception e)
        {
            logger.LogError("Commit error - {TransactionId} - Error: {Message}", dbContextTransaction.TransactionId, e.Message);
            logger.LogDebug("Rolling back transaction - {TransactionId}", dbContextTransaction.TransactionId);
            await dbContextTransaction.RollbackAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public bool HasUnsavedChanges()
    {
        return databaseContext.ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
    }
}
