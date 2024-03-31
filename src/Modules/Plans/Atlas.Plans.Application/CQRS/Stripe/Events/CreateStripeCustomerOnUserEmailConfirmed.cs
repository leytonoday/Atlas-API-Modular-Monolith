using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class CreateStripeCustomerOnUserEmailConfirmed(IStripeService stripeService, IStripeCustomerRepository stripeCustomerRepository, UserManager<User> userManager) : BaseDomainEventHandler<UserEmailConfirmedEvent, IUnitOfWork>(null)
{
    protected override async Task HandleInner(UserEmailConfirmedEvent notification, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(notification.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        var stripeCustomer = await stripeService.CreateCustomerAsync(user, cancellationToken);
        await stripeCustomerRepository.AddAsync(stripeCustomer, cancellationToken);
    }
}
