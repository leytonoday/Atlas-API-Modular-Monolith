using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.RemoveFromPlansIfHasIsInheritableChanged.FeatureUpdated;

internal sealed class RemoveFromPlansIfHasIsInheritableChangedQueuedCommandHandler(IPlanFeatureRepository planFeatureRepository, IPlanRepository planRepository) : IQueuedCommandHandler<RemoveFromPlansIfHasIsInheritableChangedQueuedCommand>
{
    public async Task Handle(RemoveFromPlansIfHasIsInheritableChangedQueuedCommand request, CancellationToken cancellationToken)
    {
        // If the "IsInheritable" property of the Feature has been changed, we're doing to remove this feature from all plans.
        // It just makes life easier when managing plan features to remove it entirely whenever this property chanegs.
        if (request.HasIsInheritableChanged)
        {
            IEnumerable<Plan> allPlans = await planRepository.GetAllAsync(true, cancellationToken);
            foreach (Plan plan in allPlans)
            {
                // Is there a plan feature on this plan, with the given feature Id?
                PlanFeature? planFeature = (await planFeatureRepository.GetByConditionAsync(x =>
                    x.PlanId == plan.Id && x.FeatureId == request.FeatureId, true, cancellationToken)).FirstOrDefault();

                // If this feature exists on this plan, remove it
                if (planFeature is not null)
                    await planFeatureRepository.RemoveAsync(planFeature, cancellationToken);
            }
        }
    }
}
