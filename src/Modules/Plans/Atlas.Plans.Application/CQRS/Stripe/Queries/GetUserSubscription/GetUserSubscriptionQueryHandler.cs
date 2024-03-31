using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.CancelSubscription;

internal sealed class GetUserSubscriptionQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, UserManager<User> userManager) : IRequestHandler<GetUserSubscriptionQuery, Subscription?>
{
    public async Task<Subscription?> Handle(GetUserSubscriptionQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        Subscription? subscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken);
        return subscription;
    }
}
