using Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeature;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeatureFromPlan;

internal sealed class RemoveFeatureFromPlanCommandHandler(IPlanFeatureRepository planFeatureRepository) : ICommandHandler<RemoveFeatureFromPlanCommand>
{
    public async Task Handle(RemoveFeatureFromPlanCommand request, CancellationToken cancellationToken)
    {
        await PlanFeature.DeleteAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            planFeatureRepository,
            cancellationToken
        );
    }
}
