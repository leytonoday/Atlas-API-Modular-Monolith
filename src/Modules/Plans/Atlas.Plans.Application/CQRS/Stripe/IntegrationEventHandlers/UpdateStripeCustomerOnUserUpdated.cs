using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Atlas.Users.IntegrationEvents;
using Atlas.Shared.Infrastructure.Integration;

namespace Atlas.Plans.Application.CQRS.Stripe.IntegrationEventHandlers;

public sealed class UpdateStripeCustomerOnUserUpdated(IStripeService stripeService) : IIntegrationEventHandler<UserUpdatedIntegrationEvent>
{
    public async Task Handle(UserUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await stripeService.UpdateCustomerAsync(notification.UserId, notification.UserName, notification.Email, notification.PhoneNumber, cancellationToken);
    }
}
