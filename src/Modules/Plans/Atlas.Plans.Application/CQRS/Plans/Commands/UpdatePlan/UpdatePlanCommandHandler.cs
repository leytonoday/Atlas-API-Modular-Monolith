using Atlas.Plans.Application.CQRS.Plans.Queue.UpdateStripeProductAndPrices;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

internal sealed class UpdatePlanCommandHandler(PlanService planService, IPlanRepository planRepository, IStripeService stripeService, IQueueWriter queueWriter) : ICommandHandler<UpdatePlanCommand>
{
    public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(request.Id, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        bool hasBeenReactivated = !plan.Active && request.Active; // Has the plan been re-activated?
        bool hasBeenDeactivated = plan.Active && !request.Active; // Has the plan de-activated?
        bool havePricesChanged = plan.MonthlyPrice != request.MonthlyPrice || plan.AnnualPrice != request.AnnualPrice;

        await Plan.UpdateAsync(
            plan,
            hasBeenDeactivated,
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
            planRepository,
            cancellationToken
        );

        await queueWriter.WriteAsync(new UpdateStripeProductAndPricesQueuedCommand(plan.Id, hasBeenDeactivated, hasBeenReactivated, havePricesChanged), cancellationToken);

        await planRepository.UpdateAsync(plan, cancellationToken);
    }
}
