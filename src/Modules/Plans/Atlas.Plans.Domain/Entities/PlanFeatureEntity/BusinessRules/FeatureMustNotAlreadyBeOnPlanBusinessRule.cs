using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanFeatureEntity.BusinessRules;

internal class FeatureMustNotAlreadyBeOnPlanBusinessRule(Plan plan, Guid featureId) : IBusinessRule
{
    public string Message => "The specified Feature already exists on the specified Plan.";

    public string ErrorCode => $"PlanFeature.{nameof(FeatureMustNotAlreadyBeOnPlanBusinessRule)}";

    public bool IsBroken()
    {
        return plan.PlanFeatures?.Any(x => x.FeatureId == featureId) ?? false;
    }
}
