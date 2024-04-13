using Atlas.Plans.Domain.Entities.FeatureEntity.DomainEvents;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging;

namespace Atlas.Plans.Application.CQRS.Features.DomainEventHandlers;

internal class RemoveFromPlansIfHasIsInheritableChangedOnFeatureUpdated(IPlanRepository planRepository) : IDomainEventHandler<FeatureUpdatedDomainEvent>
{
    public async Task Handle(FeatureUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // If the "IsInheritable" property of the Feature has been changed, we're doing to remove this feature from all plans.
        // It just makes life easier when managing plan features to remove it entirely whenever this property chanegs.
        if (notification.HasIsInheritableChanged)
        {
            IEnumerable<Plan> allPlans = await planRepository.GetAllAsync(true, cancellationToken);
            foreach (Plan plan in allPlans)
            {
                // Is there a plan feature on this plan, with the given feature Id?
                PlanFeature? planFeature = await planRepository.GetPlanFeatureAsync(plan.Id, notification.FeatureId, true, cancellationToken);

                // If this feature exists on this plan, remove it
                if (planFeature is not null)
                {
                    await planRepository.RemovePlanFeatureAsync(planFeature, cancellationToken);
                }
            }
        }
    }
}
