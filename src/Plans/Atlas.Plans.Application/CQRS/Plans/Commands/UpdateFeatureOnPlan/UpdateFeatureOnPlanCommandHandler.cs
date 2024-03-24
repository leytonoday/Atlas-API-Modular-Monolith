using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.UpdateFeatureOnPlan;

internal sealed class UpdateFeatureOnPlanCommandHandler(IPlansUnitOfWork unitOfWork) : IRequestHandler<UpdateFeatureOnPlanCommand>
{
    public async Task Handle(UpdateFeatureOnPlanCommand request, CancellationToken cancellationToken)
    {
        await PlanFeature.UpdateAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            request.Value,
            unitOfWork.PlanFeatureRepository,
            cancellationToken
        );

        await unitOfWork.CommitAsync(cancellationToken);
    }
}
