using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Domain.Entities.PlanFeatureEntity;

public class PlanFeature : Entity
{
    /// <summary>
    /// Gets or sets the Id of the <see cref="Plan"/> associated with this <see cref="PlanFeature"/>.
    /// </summary>
    public Guid PlanId { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="Plan"/> associated with this <see cref="PlanFeature"/>.
    /// </summary>
    public Guid FeatureId { get; private set; }

    /// <summary>
    /// Gets or sets the value of the feature for the plan. For example, if the feature is "Maximum number of documents", the value could be "1000"
    /// If null, it means that feature is available for the plan, but it's not something that can be quantified. So, for example, is "Translation", 
    /// it would be null, and rendered as a checkmark on the UI.
    /// </summary>
    public string? Value { get; private set; }

    public static async Task<PlanFeature> CreateAsync(
        Guid planId,
        Guid featureId,
        string? value,
        IPlanRepository planRepository,
        IFeatureRepository featureRepository,
        CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(planId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        Feature feature = await featureRepository.GetByIdAsync(featureId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Feature.FeatureNotFound);

        // Ensure this feature is not already on the plan
        if (plan.PlanFeatures is not null && plan.PlanFeatures.Any(x => x.FeatureId == featureId))
            throw new ErrorException(PlansDomainErrors.Plan.FeatureAlreadyOnPlan);

        return new PlanFeature()
        {
            PlanId = planId,
            FeatureId = featureId,
            Value = value,
        };
    }

    public static async Task DeleteAsync(
        Guid planId,
        Guid featureId,
        IPlanFeatureRepository planFeatureRepository,
        CancellationToken cancellationToken)
    {
        PlanFeature planFeature = (await planFeatureRepository.GetByConditionAsync(x => x.PlanId == planId && x.FeatureId == featureId, true, cancellationToken)).FirstOrDefault()
            ?? throw new ErrorException(PlansDomainErrors.PlanFeature.PlanFeatureNotFound);

        await planFeatureRepository.RemoveAsync(planFeature, cancellationToken);
    }

    public static async Task UpdateAsync(
        Guid planId,
        Guid featureId,
        string? value,
        IPlanFeatureRepository planFeatureRepository,
        CancellationToken cancellationToken)
    {
        PlanFeature planFeature = (await planFeatureRepository.GetByConditionAsync(x => x.PlanId == planId && x.FeatureId == featureId, true, cancellationToken)).FirstOrDefault()
            ?? throw new ErrorException(PlansDomainErrors.PlanFeature.PlanFeatureNotFound);

        planFeature.PlanId = planId;
        planFeature.FeatureId = featureId;
        planFeature.Value = value;

        await planFeatureRepository.UpdateAsync(planFeature, cancellationToken);
    }
}
