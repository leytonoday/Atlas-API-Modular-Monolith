using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.UpdateFeatureOnPlan;

internal sealed class UpdateFeatureOnPlanCommandHandler(IPlanFeatureRepository planFeatureRepository) : ICommandHandler<UpdateFeatureOnPlanCommand>
{
    public async Task Handle(UpdateFeatureOnPlanCommand request, CancellationToken cancellationToken)
    {
        await PlanFeature.UpdateAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            request.Value,
            planFeatureRepository,
            cancellationToken
        );
    }
}
