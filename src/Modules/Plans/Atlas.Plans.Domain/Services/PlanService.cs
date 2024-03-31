using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;

namespace Atlas.Plans.Domain.Services;

public sealed class PlanService(IPlanRepository planRepository, IFeatureRepository featureRepository)
{
    /// <summary>
    /// Determines if a <see cref="Plan"/> with the name of <paramref name="name"/> already exists.
    /// </summary>
    /// <param name="name">The name to check for.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns if the plan exists or not.</returns>
    public async Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken)
    {
        return await planRepository.IsNameTakenAsync(name, cancellationToken);
    }

    /// <summary>
    /// Determins whether the InheritsFromId property of the <see cref="Plan"/> would cause a circular reference.
    /// </summary>
    /// <param name="plan">The <see cref="Plan"/> to check.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a <c>true</c> if circular inheritance is detected, and <c>false</c> otherwise.</returns>
    public async Task<bool> IsCircularInheritanceDetectedAsync(Plan plan, CancellationToken cancellationToken)
    {
        var parentPlanIds = new List<Guid>();
        await GetParentPlanIdsAsync(plan, parentPlanIds, cancellationToken);
        return parentPlanIds.Contains(plan.Id);
    }

    /// <summary>
    /// Recursively gets a hierarchy of all <see cref="Plan"/> ids, following up the chain of InheritsFromId properties.
    /// </summary>
    /// <param name="currentPlan">The <see cref="Plan"/> for which to find the parents.</param>
    /// <param name="parentPlanIds">The list of parent Ids.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a list of the <paramref name="currentPlan"/>'s parents</returns>
    private async Task GetParentPlanIdsAsync(Plan currentPlan, List<Guid> parentPlanIds, CancellationToken cancellationToken)
    {
        if (!currentPlan.InheritsFromId.HasValue)
            return;

        Plan? parentPlan = (await planRepository.GetByIdAsync(currentPlan.InheritsFromId.Value, false, cancellationToken))!;
        if (parentPlan is null)
            return;

        parentPlanIds.Add(parentPlan.Id);

        if (parentPlan.InheritsFromId.HasValue)
            await GetParentPlanIdsAsync(parentPlan, parentPlanIds, cancellationToken);
    }

    public async Task<IEnumerable<Feature>> GetInheritedFeatures(Plan plan, CancellationToken cancellationToken)
    {
        // Plans can inherit from one another, to inherit features, we need to recursively get all the features
        var allFeatures = new List<Feature>();

        if (plan.InheritsFromId.HasValue)
            await RecursivelyGetFeaturesAsync(plan.InheritsFromId.Value, allFeatures, cancellationToken);

        return allFeatures.ToList();
    }

    /// <summary>
    /// Recursively gets all the <see cref="Feature"/>s for a <see cref="Plan"/> and its parents
    /// </summary>
    /// <param name="planId">The Id of the <see cref="Plan"/> for which to fetch the <see cref="Feature"/>s.</param>
    /// <param name="features">The list of currently retrieved <see cref="Feature"/>s.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    /// <remarks>If a <see cref="Plan"/> has an <see cref="Plan.InheritsFromId"/>, then this method will recursively go to that <see cref="Plan"/>, 
    /// and get all of it's <see cref="Feature"/>s that are inheritable.</remarks>
    private async Task RecursivelyGetFeaturesAsync(Guid planId, List<Feature> features, CancellationToken cancellationToken)
    {
        Plan? plan = await planRepository.GetByIdAsync(planId, true, cancellationToken);
        if (plan is null)
            return;

        if (plan.Features is not null)
        {
            var inheritableFeatures = plan.Features.Where(x => x.IsInheritable);
            features.AddRange(inheritableFeatures);
        }

        if (plan.InheritsFromId.HasValue)
            await RecursivelyGetFeaturesAsync(plan.InheritsFromId.Value, features, cancellationToken);
    }

    public async Task<IEnumerable<PlanFeature>> GetInheritedPlanFeatures(Plan plan, CancellationToken cancellationToken)
    {
        // Plans can inherit from one another, to inherit features, we need to recursively get all the features
        var allPlanFetures = new List<PlanFeature>();

        if (plan.InheritsFromId.HasValue)
            await RecursivelyGetPlanFeaturesAsync(plan.InheritsFromId.Value, allPlanFetures, cancellationToken);

        return allPlanFetures;
    }

    /// <summary>
    /// Recursively gets all the <see cref="PlanFeature"/>s for a <see cref="Plan"/> and its parents.
    /// </summary>
    /// <param name="planId">The Id of the <see cref="Plan"/> for which to fetch the <see cref="PlanFeature"/>s.</param>
    /// <param name="planFeatures">The list of currently retrieved <see cref="PlanFeature"/>s.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    /// <remarks>If a <see cref="Plan"/> has an <see cref="Plan.InheritsFromId"/>, then this method will recursively go to that <see cref="Plan"/>, 
    /// and get all of it's <see cref="PlanFeature"/>s, where the <see cref="Feature"/> is inheritable.</remarks>
    private async Task RecursivelyGetPlanFeaturesAsync(Guid planId, List<PlanFeature> planFeatures, CancellationToken cancellationToken)
    {
        Plan? plan = await planRepository.GetByIdAsync(planId, true, cancellationToken);
        if (plan is null)
            return;

        if (plan.PlanFeatures is not null)
        {
            IEnumerable<PlanFeature> planFeaturesToAdd = plan.PlanFeatures.ToList();
            var inheritablePlanFeatures = new List<PlanFeature>();

            foreach (PlanFeature planFeatureToAdd in planFeaturesToAdd)
            {
                // Only add the plan feature if the feature it links to is marked as inheritable
                Feature? feature = await featureRepository.GetByIdAsync(planFeatureToAdd.FeatureId, false, cancellationToken);
                if (feature is not null && feature.IsInheritable)
                {
                    inheritablePlanFeatures.Add(planFeatureToAdd);
                }
            }

            planFeatures.AddRange(inheritablePlanFeatures);
        }

        if (plan.InheritsFromId.HasValue)
            await RecursivelyGetPlanFeaturesAsync(plan.InheritsFromId.Value, planFeatures, cancellationToken);
    }
}
