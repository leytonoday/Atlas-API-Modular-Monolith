using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.AddFeatureToPlan;

internal sealed class AddFeatureToPlanCommandHandler(IPlanRepository planRepository, IFeatureRepository featureRepository) : ICommandHandler<AddFeatureToPlanCommand>
{
    public async Task Handle(AddFeatureToPlanCommand request, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(request.PlanId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        Feature feature = await featureRepository.GetByIdAsync(request.FeatureId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Feature.FeatureNotFound);     

        await Plan.AddPlanFeatureAsync(plan, feature, request.Value, planRepository, cancellationToken);
    }
}
