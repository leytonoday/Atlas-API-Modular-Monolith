using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanFeatureEntity.BusinessRules;

/// <summary>
/// A business rule ensuring that a feature is not already associated with a plan.
/// </summary>
/// <param name="plan">The plan to check for the existence of the feature.</param>
/// <param name="featureId">The ID of the feature to check.</param>
internal class FeatureMustNotAlreadyBeOnPlanBusinessRule(Plan plan, Guid featureId) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "The specified Feature already exists on the specified Plan.";

    /// <inheritdoc/>
    public string Code => $"PlanFeature.{nameof(FeatureMustNotAlreadyBeOnPlanBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        // Check if any feature with the specified ID already exists on the plan.
        return plan.PlanFeatures?.Any(x => x.FeatureId == featureId) ?? false;
    }
}
