using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanEntity.Events;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Plans.Events;

public sealed class CreateStripeProductOnPlanCreated(IPlansUnitOfWork unitOfWork, IStripeService stripeService) : BaseDomainEventHandler<PlanCreatedEvent, IPlansUnitOfWork>(unitOfWork)
{
    protected override async Task HandleInner(PlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        Plan plan = await unitOfWork.PlanRepository.GetByNameAsync(notification.PlanName, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        await Plan.CreateStripeEntities(plan, stripeService, cancellationToken);

        await unitOfWork.PlanRepository.UpdateAsync(plan, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
