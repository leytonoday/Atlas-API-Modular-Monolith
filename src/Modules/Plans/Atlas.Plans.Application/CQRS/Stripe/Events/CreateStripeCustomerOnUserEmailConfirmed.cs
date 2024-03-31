using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.IntegrationEvents;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class CreateStripeCustomerOnUserEmailConfirmed(IStripeService stripeService, IStripeCustomerRepository stripeCustomerRepository, UserManager<User> userManager) : IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>
{
    protected async Task Handle(UserEmailConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(notification.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        var stripeCustomer = await stripeService.CreateCustomerAsync(user, cancellationToken);
        await stripeCustomerRepository.AddAsync(stripeCustomer, cancellationToken);
    }
}
