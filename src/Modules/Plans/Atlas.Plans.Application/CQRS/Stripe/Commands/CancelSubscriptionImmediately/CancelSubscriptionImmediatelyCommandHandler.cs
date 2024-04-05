using Stripe;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscriptionImmediately;

internal sealed class CancelSubscriptionImmediatelyCommandHandler(IStripeCustomerRepository stripeCustomerRepository, IExecutionContextAccessor executionContext, IStripeService stripeService) : ICommandHandler<CancelSubscriptionImmediatelyCommand>
{
    public async Task Handle(CancelSubscriptionImmediatelyCommand request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        Subscription subscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        await stripeService.CancelSubscriptionImmediatelyAsync(subscription.Id, cancellationToken);
    }
}
