using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.AddFeatureToPlan;

internal sealed class AddFeatureToPlanCommandHandler(IPlansUnitOfWork unitOfWork) : ICommandHandler<AddFeatureToPlanCommand>
{
    public async Task Handle(AddFeatureToPlanCommand request, CancellationToken cancellationToken)
    {
        var planFeature = await PlanFeature.CreateAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            value: request.Value,
            unitOfWork.PlanRepository,
            unitOfWork.FeatureRepository,
            cancellationToken
        );

        await unitOfWork.PlanFeatureRepository.AddAsync(planFeature, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
