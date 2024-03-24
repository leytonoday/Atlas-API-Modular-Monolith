using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanEntity.Events;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Events;

internal sealed class PlanCreatedEventHandler(IPlansUnitOfWork unitOfWork, IStripeService stripeService) : INotificationHandler<PlanCreatedEvent>
{
    public async Task Handle(PlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        Plan plan = await unitOfWork.PlanRepository.GetByNameAsync(notification.PlanName, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        await Plan.CreateStripeEntities(plan, stripeService, cancellationToken);

        await unitOfWork.PlanRepository.UpdateAsync(plan, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
