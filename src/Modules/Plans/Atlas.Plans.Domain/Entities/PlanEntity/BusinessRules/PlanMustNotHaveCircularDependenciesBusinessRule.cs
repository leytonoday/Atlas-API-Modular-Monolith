using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanEntity.BusinessRules;

internal sealed class PlanMustNotHaveCircularDependenciesBusinessRule(PlanService planService, Plan plan) : IAsyncBusinessRule
{
    public string Message => "The InheritsFromId value provided causes a circular dependence.";

    public string ErrorCode => $"Plan.{nameof(PlanMustNotHaveCircularDependenciesBusinessRule)}";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        // If it isn't inheriting from anything, then this rule can't possibly be violated
        if (!plan.InheritsFromId.HasValue)
        {
            return false;
        }

        return await planService.IsCircularInheritanceDetectedAsync(plan, cancellationToken);
    }
}
