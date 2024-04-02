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
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.ReactivateSubscription;

internal sealed class ReactivateSubscriptionCommandHandler(IStripeCustomerRepository stripeCustomerRepository, IExecutionContextAccessor executionContext, IStripeService stripeService) : ICommandHandler<ReactivateSubscriptionCommand>
{
    public async Task Handle(ReactivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await stripeService.SetSubscriptionCancelAtPeriodEnd(stripeCustomer.StripeCustomerId, false, cancellationToken);
    }
}
