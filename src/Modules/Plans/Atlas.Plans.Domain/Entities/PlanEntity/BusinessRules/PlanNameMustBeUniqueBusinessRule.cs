using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanEntity.BusinessRules;

internal class PlanNameMustBeUniqueBusinessRule(string name, IPlanRepository planRepository) : IAsyncBusinessRule
{
    public string Message => "The Plan name must be unique";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        return await planRepository.IsNameTakenAsync(name, cancellationToken);
    }
}
