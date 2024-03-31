using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanEntity.Events;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Plans.Events;

public sealed class UpdateStripeProductAndPlandOnPlanUpdated(IStripeService stripeService, IPlanRepository planRepository) : BaseDomainEventHandler<PlanUpdatedEvent, IUnitOfWork>(null)
{
    protected override async Task HandleInner(PlanUpdatedEvent notification, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(notification.PlanId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        // Update the product
        await stripeService.UpdateProductAsync(plan, cancellationToken);

        // If the plan has been deactivated, we're just going to deactivate all prices
        if (notification.HasBeenDeactivated)
        {
            await stripeService.DeactivateAllPricesAsync(plan, cancellationToken);
        }
        // If the plan has been reactivated (previously inactive, then made active again) create new prices
        else if (notification.HasBeenReactivated)
        {
            await stripeService.CreatePricesForPlanAsync(plan, cancellationToken);
        }

        // If the prices have changed, deactive and then create new prices
        if (notification.HavePricesChanged)
        {
            await stripeService.UpgradePriceAtIntervalAsync(plan, "month", cancellationToken);
            await stripeService.UpgradePriceAtIntervalAsync(plan, "year", cancellationToken);
        }
    }
}
