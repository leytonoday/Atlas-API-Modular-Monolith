using Atlas.Plans.Domain.Entities.PlanEntity.Events;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class UpdateStripeCustomerOnUserUpdated(IStripeService stripeService, UserManager<User> userManager, IPlansUnitOfWork unitOfWork) : BaseDomainEventHandler<UserUpdatedEvent, IPlansUnitOfWork>(unitOfWork)
{
    protected override async Task HandleInner(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(notification.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await stripeService.UpdateCustomerAsync(user, cancellationToken);
    }
}
