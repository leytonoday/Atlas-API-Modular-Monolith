using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Plans.Queue.CreateStripeProductAndPrices;

internal sealed class CreateStripeProductAndPricesQueuedCommandHandler(IPlanRepository planRepository, IStripeService stripeService) : IQueuedCommandHandler<CreateStripeProductAndPricesQueuedCommand>
{
    public async Task Handle(CreateStripeProductAndPricesQueuedCommand request, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByNameAsync(request.PlanName, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        await Plan.CreateStripeEntities(plan, stripeService, cancellationToken);

        await planRepository.UpdateAsync(plan, cancellationToken);
    }
}
