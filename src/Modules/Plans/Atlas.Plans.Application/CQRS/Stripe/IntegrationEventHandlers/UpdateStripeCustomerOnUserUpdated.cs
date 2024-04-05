using Atlas.Plans.Domain.Services;
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
