using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Atlas.Users.IntegrationEvents;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class UpdateStripeCustomerOnUserUpdated(IStripeService stripeService, UserManager<User> userManager) : INotificationHandler<UserUpdatedIntegrationEvent>
{
    public async Task Handle(UserUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(notification.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await stripeService.UpdateCustomerAsync(user, cancellationToken);
    }
}
