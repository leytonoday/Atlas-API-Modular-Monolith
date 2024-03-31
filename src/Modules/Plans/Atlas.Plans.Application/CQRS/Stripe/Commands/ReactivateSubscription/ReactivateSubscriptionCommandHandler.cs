using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Errors;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.ReactivateSubscription;

internal sealed class ReactivateSubscriptionCommandHandler(IStripeCustomerRepository stripeCustomerRepository, UserManager<User> userManager, IStripeService stripeService) : IRequestHandler<ReactivateSubscriptionCommand>
{
    public async Task Handle(ReactivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await stripeService.SetSubscriptionCancelAtPeriodEnd(stripeCustomer.StripeCustomerId, false, cancellationToken);
    }
}
