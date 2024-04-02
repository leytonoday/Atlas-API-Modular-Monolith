using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queue.UpdateStripeProductAndPrices;

internal sealed class UpdateStripeProductAndPricesQueuedCommandHandler(IStripeService stripeService, IPlanRepository planRepository) : IQueuedCommandHandler<UpdateStripeProductAndPricesQueuedCommand>
{
    public async Task Handle(UpdateStripeProductAndPricesQueuedCommand request, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(request.PlanId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        // Update the product
        await stripeService.UpdateProductAsync(plan, cancellationToken);

        // If the plan has been deactivated, we're just going to deactivate all prices
        if (request.HasBeenDeactivated)
        {
            await stripeService.DeactivateAllPricesAsync(plan, cancellationToken);
        }
        // If the plan has been reactivated (previously inactive, then made active again) create new prices
        else if (request.HasBeenReactivated)
        {
            await stripeService.CreatePricesForPlanAsync(plan, cancellationToken);
        }

        // If the prices have changed, deactive and then create new prices
        if (request.HavePricesChanged)
        {
            await stripeService.UpgradePriceAtIntervalAsync(plan, "month", cancellationToken);
            await stripeService.UpgradePriceAtIntervalAsync(plan, "year", cancellationToken);
        }
    }
}
