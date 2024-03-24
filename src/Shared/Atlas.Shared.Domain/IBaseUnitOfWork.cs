namespace Atlas.Shared.Domain;

public interface IBaseUnitOfWork
{
    /// <summary>
    /// Commits the changes made within the unit of work to the underlying database.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
