using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Users.IntegrationEvents;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class CreateStripeCustomerOnUserEmailConfirmed(IStripeService stripeService, IStripeCustomerRepository stripeCustomerRepository) : INotificationHandler<UserEmailConfirmedIntegrationEvent>
{
    public async Task Handle(UserEmailConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var stripeCustomer = await stripeService.CreateCustomerAsync(notification.UserId, notification.UserName, notification.Email, notification.PhoneNumber, cancellationToken);
        await stripeCustomerRepository.AddAsync(stripeCustomer, cancellationToken);
    }
}
