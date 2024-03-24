using Atlas.Shared.Domain.Persistance;

namespace Atlas.Plans.Domain.Entities.PlanFeatureEntity;

/// <summary>
/// Represents a repository for managing <see cref="PlanFeature"/> entities.
/// </summary>
public interface IPlanFeatureRepository : IRepository<PlanFeature>
{
    public Task<IEnumerable<PlanFeature>> GetByPlanId(Guid planId, bool trackChanges, CancellationToken cancellationToken);
}