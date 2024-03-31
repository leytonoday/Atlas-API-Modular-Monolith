using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

internal sealed class CreatePlanCommandHandler(PlanService planService, IPlanRepository planRepository) : ICommandHandler<CreatePlanCommand, Guid>
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
            planService,
            cancellationToken
        );

        await planRepository.AddAsync(plan, cancellationToken);

        return plan.Id;
    }
}
