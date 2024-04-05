using Stripe;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUpcomingInvoice;

internal sealed class GetUpcomingInvoiceQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, IExecutionContextAccessor executionContext) : IQueryHandler<GetUpcomingInvoiceQuery, Invoice>
{
    public async Task<Invoice> Handle(GetUpcomingInvoiceQuery request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        // Get the user's current subscription
        Subscription currentSubscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        // Get the upcoming invoice
        Invoice upcomingInvoice = await stripeService.InvoiceService.UpcomingAsync(new UpcomingInvoiceOptions()
        {
            Customer = stripeCustomer.StripeCustomerId,
            Subscription = currentSubscription.Id,
        }, cancellationToken: cancellationToken);

        return upcomingInvoice;
    }
}
