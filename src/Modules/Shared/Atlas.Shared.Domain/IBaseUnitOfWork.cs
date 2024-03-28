namespace Atlas.Shared.Domain;

public interface IBaseUnitOfWork
{
    /// <summary>
    /// Commits the changes made within the unit of work to the underlying database.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines if the domain event with id <paramref name="domainEventId"/> has been successfully handled by the <see cref="IDomainEventHandler"/> with name of <paramref name="eventHandlerName"/>
    /// </summary>
    /// <param name="eventHandlerName">The name of the domain event handler to check for.</param>
    /// <param name="domainEventId">The Id of the domain event to check for.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>True if it's been successfully handled previously, false otherwise.</returns>
    public Task<bool> HasDomainEventBeenHandledAsync(string eventHandlerName, Guid domainEventId, CancellationToken cancellationToken);

    public void MarkDomainEventAsHandled(string eventHandlerName, Guid domainEventId, CancellationToken cancellationToken);
}
