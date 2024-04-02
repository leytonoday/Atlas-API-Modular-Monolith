using Stripe;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscription;

internal sealed class CancelSubscriptionCommandHandler(IStripeCustomerRepository stripeCustomerRepository, IExecutionContextAccessor executionContext, IStripeService stripeService) : ICommandHandler<CancelSubscriptionCommand>
{
    public async Task Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        Subscription subscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        // If the subscription is active or past_due, and a cancellation has been requested, we set the cancel_at_period_end to true.
        // This will cancel the subscription at the end of the current billing period.
        if ((subscription.Status == SubscriptionStatuses.Active || subscription.Status == SubscriptionStatuses.PastDue) && stripeCustomer.StripeCustomerId is not null)
        {
            await stripeService.SetSubscriptionCancelAtPeriodEnd(stripeCustomer.StripeCustomerId, true, cancellationToken);
        }
        // If the user is on a trial, just cancel immediately
        if (subscription.Status == SubscriptionStatuses.Trialing && stripeCustomer.StripeCustomerId is not null)
        {
            await stripeService.CancelSubscriptionImmediatelyAsync(subscription.Id, cancellationToken);
        }
    }
}
