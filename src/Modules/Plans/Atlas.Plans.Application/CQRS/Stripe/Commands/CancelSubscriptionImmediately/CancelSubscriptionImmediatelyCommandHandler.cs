﻿using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscriptionImmediately;

internal sealed class CancelSubscriptionImmediatelyCommandHandler(IPlansUnitOfWork unitOfWork, UserManager<User> userManager, IStripeService stripeService) : IRequestHandler<CancelSubscriptionImmediatelyCommand>
{
    public async Task Handle(CancelSubscriptionImmediatelyCommand request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await unitOfWork.StripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        Subscription subscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        await stripeService.CancelSubscriptionImmediatelyAsync(subscription.Id, cancellationToken);
    }
}