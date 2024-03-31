using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.AddFeatureToPlan;

internal sealed class AddFeatureToPlanCommandHandler(IPlanFeatureRepository planFeatureRepository, IPlanRepository planRepository, IFeatureRepository featureRepository) : ICommandHandler<AddFeatureToPlanCommand>
{
    public async Task Handle(AddFeatureToPlanCommand request, CancellationToken cancellationToken)
    {
        var planFeature = await PlanFeature.CreateAsync(
            planId: request.PlanId,
            featureId: request.FeatureId,
            value: request.Value,
            planRepository,
            featureRepository,
            cancellationToken
        );

        await planFeatureRepository.AddAsync(planFeature, cancellationToken);
    }
}
