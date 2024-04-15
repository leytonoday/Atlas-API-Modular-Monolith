using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanEntity.BusinessRules;

/// <summary>
/// A business rule ensuring that a plan name is unique.
/// </summary>
/// <param name="name">The name of the plan.</param>
/// <param name="planRepository">The repository used to check for the existence of other plans.</param>
internal class PlanNameMustBeUniqueBusinessRule(string name, IPlanRepository planRepository) : IAsyncBusinessRule
{
    /// <inheritdoc/>
    public string Message => "The Plan name must be unique";

    /// <inheritdoc/>
    public string Code => $"Plan.{nameof(PlanNameMustBeUniqueBusinessRule)}";

    /// <inheritdoc/>
    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        return await planRepository.IsNameTakenAsync(name, cancellationToken);
    }
}
