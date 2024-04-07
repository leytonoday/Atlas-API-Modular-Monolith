using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Users.IntegrationEvents;

namespace Atlas.Plans.Application.CQRS.Stripe.IntegrationEventHandlers;

public sealed class DeleteStripeCustomerOnUserDeleted(IStripeService stripeService, IStripeCustomerRepository stripeCustomerRepository) : IIntegrationEventHandler<UserDeletedIntegrationEvent>
{
    public async Task Handle(UserDeletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        StripeCustomer stripeCustomer = await stripeCustomerRepository.GetByUserId(notification.UserId, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await stripeService.DeleteCustomerAsync(stripeCustomer.StripeCustomerId, cancellationToken);

        await stripeCustomerRepository.RemoveAsync(stripeCustomer, cancellationToken);
    }
}
