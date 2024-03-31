using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanEntity.Events;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Plans.Events;

public sealed class CreateStripeProductOnPlanCreated(IPlanRepository planRepository, IStripeService stripeService) : BaseDomainEventHandler<PlanCreatedEvent, IUnitOfWork>(null)
{
    protected override async Task HandleInner(PlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByNameAsync(notification.PlanName, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        await Plan.CreateStripeEntities(plan, stripeService, cancellationToken);

        await planRepository.UpdateAsync(plan, cancellationToken);
    }
}
