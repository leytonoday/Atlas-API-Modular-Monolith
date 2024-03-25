using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain.Events;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class DeleteStripeCustomerOnUserDeleted(IStripeService stripeService, IPlansUnitOfWork unitOfWork, UserManager<User> userManager) : IDomainEventHandler<UserDeletedEvent>
{
    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        StripeCustomer stripeCustomer = await unitOfWork.StripeCustomerRepository.GetByUserId(notification.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await stripeService.DeleteCustomerAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }
}
