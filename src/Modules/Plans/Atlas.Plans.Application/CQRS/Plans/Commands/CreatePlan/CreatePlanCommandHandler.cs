﻿using Atlas.Plans.Application.CQRS.Plans.Queue.CreateStripeProductAndPrices;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

internal sealed class CreatePlanCommandHandler(IPlanRepository planRepository, IQueueWriter queueWriter) : ICommandHandler<CreatePlanCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await Plan.CreateAsync(
            name: request.Name,
            description: request.Description,
            isoCurrencyCode: request.IsoCurrencyCode,
            monthyPrice: request.MonthlyPrice,
            annualPrice: request.AnnualPrice,
            trialPeriodDays: request.TrialPeriodDays,
            tag: request.Tag,
            icon: request.Icon,
            iconColour: request.IconColour,
            backgroundColour: request.BackgroundColour,
            textColour: request.TextColour,
            active: request.Active,
            inheritsFromId: request.InheritsFromId,
            planRepository,
            cancellationToken
        );

        await planRepository.AddAsync(plan, cancellationToken);

        await queueWriter.WriteAsync(new CreateStripeProductAndPricesQueuedCommand(plan.Name), cancellationToken);

        return plan.Id;
    }
}
