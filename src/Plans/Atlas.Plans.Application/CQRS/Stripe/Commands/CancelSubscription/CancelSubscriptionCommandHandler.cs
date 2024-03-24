using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Plans.Domain;
using Atlas.Users.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscription;

internal sealed class CancelSubscriptionCommandHandler(IPlansUnitOfWork unitOfWork, UserManager<User> userManager, IStripeService stripeService) : IRequestHandler<CancelSubscriptionCommand>
{
    public async Task Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await unitOfWork.StripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
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
