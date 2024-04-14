using Atlas.Shared.Domain.Persistance;

namespace Atlas.Plans.Domain.Entities.CreditTrackerEntity;

/// <summary>
/// Represents a repository for managing <see cref="CreditTracker"/> entities.
/// </summary>
public interface ICreditTrackerRepository : IRepository<CreditTracker>
{
    public Task<CreditTracker?> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);
}