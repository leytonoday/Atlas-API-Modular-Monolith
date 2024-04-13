using Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeature;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeatureFromPlan;

internal sealed class RemoveFeatureFromPlanCommandHandler(IPlanRepository planRepository) : ICommandHandler<RemoveFeatureFromPlanCommand>
{
    public async Task Handle(RemoveFeatureFromPlanCommand request, CancellationToken cancellationToken)
    {
        PlanFeature? planFeature = await planRepository.GetPlanFeatureAsync(request.PlanId, request.FeatureId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.PlanFeature.PlanFeatureNotFound);

        await planRepository.RemovePlanFeatureAsync(planFeature, cancellationToken);
    }
}
