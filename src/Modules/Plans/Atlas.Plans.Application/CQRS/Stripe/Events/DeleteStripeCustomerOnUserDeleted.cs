﻿using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.IntegrationEvents;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class DeleteStripeCustomerOnUserDeleted(IStripeService stripeService, IStripeCustomerRepository stripeCustomerRepository) : INotificationHandler<UserDeletedIntegrationEvent>
{
    public async Task Handle(UserDeletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        StripeCustomer stripeCustomer = await stripeCustomerRepository.GetByUserId(notification.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await stripeService.DeleteCustomerAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }
}
