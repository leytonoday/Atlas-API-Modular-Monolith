using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

internal sealed class UpdatePlanCommandHandler(PlanService planService, IPlansUnitOfWork unitOfWork, IStripeService stripeService) : ICommandHandler<UpdatePlanCommand>
{
    public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await Plan.UpdateAsync(
            id: request.Id,
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
            stripeService,
            unitOfWork.PlanRepository,
            cancellationToken
        );

        await unitOfWork.PlanRepository.UpdateAsync(plan, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
