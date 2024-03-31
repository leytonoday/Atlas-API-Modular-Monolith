using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Stripe.Events;

public sealed class DeleteStripeCustomerOnUserDeleted(IStripeService stripeService, IStripeCustomerRepository stripeCustomerRepository) : BaseDomainEventHandler<UserDeletedEvent, IUnitOfWork>(null)
{
    protected override async Task HandleInner(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        StripeCustomer stripeCustomer = await stripeCustomerRepository.GetByUserId(notification.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await stripeService.DeleteCustomerAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }
}
