using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanEntity.BusinessRules;

/// <summary>
/// A business rule ensuring that a plan does not have circular dependencies.
/// </summary>
/// <param name="planService">The service used to check for circular dependencies.</param>
/// <param name="plan">The plan to be checked for circular dependencies.</param>
internal sealed class PlanMustNotHaveCircularDependenciesBusinessRule(PlanService planService, Plan plan) : IAsyncBusinessRule
{
    /// <inheritdoc/>
    public string Message => "The InheritsFromId value provided causes a circular dependence.";

    /// <inheritdoc/>
    public string Code => $"Plan.{nameof(PlanMustNotHaveCircularDependenciesBusinessRule)}";

    /// <inheritdoc/>
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
