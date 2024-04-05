using Stripe;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserPaymentMethodsQuery;

internal sealed class GetUserPaymentMethodsQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, IExecutionContextAccessor executionContext) : IQueryHandler<GetUserPaymentMethodsQuery, IEnumerable<PaymentMethod>>
{
    public async Task<IEnumerable<PaymentMethod>> Handle(GetUserPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        var paymentMethodListOptions = new PaymentMethodListOptions
        {
            Customer = stripeCustomer.StripeCustomerId,
            Type = "card"
        };

        IEnumerable<PaymentMethod> paymentMethods = await stripeService.PaymentMethodService.ListAsync(paymentMethodListOptions, cancellationToken: cancellationToken);

        return paymentMethods;
    }
}
