using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.PlanEntity.BusinessRules;

/// <summary>
/// A plan cannot be de-activated if it has active subscribers
/// </summary>
/// <param name="hasBeenDeactivated">Has this <see cref="Plan"/> been deactivated?</param>
/// <param name="stripeService"></param>
/// <param name="plan">The <see cref="Plan"/> to delete.</param>
internal class PlanMustHaveNoActiveSubscriptionsToBeDeactivatedBusinessRule(bool hasBeenDeactivated, IStripeService stripeService, Plan plan) : IAsyncBusinessRule
{
    public string Message => "A Plan cannot be deactivated when it has active Subscribers.";

    public string ErrorCode => $"Plan.{nameof(PlanMustHaveNoActiveSubscriptionsToBeDeactivatedBusinessRule)}";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        if (!hasBeenDeactivated)
        {
            return false;
        }

        return await stripeService.DoesPlanHaveActiveSubscriptions(plan, cancellationToken);
    }
}
