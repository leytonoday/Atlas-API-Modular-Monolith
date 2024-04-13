using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Domain.Persistance;

namespace Atlas.Plans.Domain.Entities.PlanEntity;

/// <summary>
/// Represents a repository for managing <see cref="Plan"/> entities.
/// </summary>
public interface IPlanRepository : IRepository<Plan, Guid>
{
    public Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a <see cref="Plan"/> which has an InheritsFromId value the specified <see cref="Plan"/> Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Plan"/> that the target <see cref="Plan"/> inherits from.</param>
    /// <param name="trackChanges">Determines whether to track changes for the retrieved <see cref="Plan"/> entity.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a <see cref="Plan"/> entity.</returns>
    public Task<Plan?> GetByInheritsFromIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a <see cref="Plan"/> by its name.
    /// </summary>
    /// <param name="name">The name of the <see cref="Plan"/> to get.</param>
    /// <param name="trackChanges">Determines whether to track changes for the retrieved <see cref="Plan"/> entity.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a <see cref="Plan"/> entity.</returns>
    public Task<Plan?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken);

    public Task AddPlanFeatureAsync(PlanFeature planFeature, CancellationToken cancellationToken);

    public Task RemovePlanFeatureAsync(PlanFeature planFeature, CancellationToken cancellationToken);

    public Task UpdatePlanFeatureAsync(PlanFeature planFeature, CancellationToken cancellationToken);

    public Task<PlanFeature?> GetPlanFeatureAsync(Guid planId, Guid featureId, bool trackChanges, CancellationToken cancellationToken);
}