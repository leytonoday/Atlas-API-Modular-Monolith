using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.UpdateFeatureOnPlan;

internal sealed class UpdateFeatureOnPlanCommandHandler(IPlanRepository planRepository) : ICommandHandler<UpdateFeatureOnPlanCommand>
{
    public async Task Handle(UpdateFeatureOnPlanCommand request, CancellationToken cancellationToken)
    {
        await PlanFeature.UpdateAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            request.Value,
            planRepository,
            cancellationToken
        );
    }
}
