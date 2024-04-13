using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity.BusinessRules;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Domain.Entities.PlanFeatureEntity;

public class PlanFeature : Entity
{
    private PlanFeature() { }

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

    public static PlanFeature Create(
        Plan plan,
        Feature feature,
        string? value)
    {
        // Ensure this feature is not already on the plan
        CheckBusinessRule(new FeatureMustNotAlreadyBeOnPlanBusinessRule(plan, feature.Id));

        return new PlanFeature()
        {
            PlanId = plan.Id,
            FeatureId = feature.Id,
            Value = value,
        };
    }

    public static async Task UpdateAsync(
        Guid planId,
        Guid featureId,
        string? value,
        IPlanRepository planRepository,
        CancellationToken cancellationToken)
    {
        PlanFeature planFeature = await planRepository.GetPlanFeatureAsync(planId, featureId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.PlanFeature.PlanFeatureNotFound);

        planFeature.PlanId = planId;
        planFeature.FeatureId = featureId;
        planFeature.Value = value;

        await planRepository.UpdatePlanFeatureAsync(planFeature, cancellationToken);
    }
}
