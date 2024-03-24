﻿using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity.Events;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging;

namespace Atlas.Plans.Infrastructure.CQRS.Features.Events;

internal sealed class FeatureCreatedEventHandler(IPlansUnitOfWork unitOfWork) : IDomainEventHandler<FeatureUpdatedEvent>
{
    public async Task Handle(FeatureUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // If the "IsInheritable" property of the Feature has been changed, we're doing to remove this feature from all plans.
        // It just makes life easier when managing plan features to remove it entirely whenever this property chanegs.
        if (notification.HasIsInheritableChanged)
        {
            IEnumerable<Plan> allPlans = await unitOfWork.PlanRepository.GetAllAsync(true, cancellationToken);
            foreach (Plan plan in allPlans)
            {
                // Is there a plan feature on this plan, with the given feature Id?
                PlanFeature? planFeature = (await unitOfWork.PlanFeatureRepository.GetByConditionAsync(x =>
                    x.PlanId == plan.Id && x.FeatureId == notification.Id, true, cancellationToken)).FirstOrDefault();

                // If this feature exists on this plan, remove it
                if (planFeature is not null)
                    await unitOfWork.PlanFeatureRepository.RemoveAsync(planFeature, cancellationToken);
            }
        }
    }
}
