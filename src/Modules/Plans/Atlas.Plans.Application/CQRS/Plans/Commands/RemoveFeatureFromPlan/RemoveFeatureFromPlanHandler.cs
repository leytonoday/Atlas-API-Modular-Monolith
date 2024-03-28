using Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeature;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeatureFromPlan;

internal sealed class RemoveFeatureFromPlanCommandHandler(IPlansUnitOfWork unitOfWork) : IRequestHandler<RemoveFeatureFromPlanCommand>
{
    public async Task Handle(RemoveFeatureFromPlanCommand request, CancellationToken cancellationToken)
    {
        await PlanFeature.DeleteAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            unitOfWork.PlanFeatureRepository,
            cancellationToken
        );

        await unitOfWork.CommitAsync(cancellationToken);
    }
}
