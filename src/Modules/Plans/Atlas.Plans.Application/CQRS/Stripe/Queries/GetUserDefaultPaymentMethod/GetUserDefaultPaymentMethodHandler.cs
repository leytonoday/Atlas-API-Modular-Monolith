using Stripe;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserDefaultPaymentMethod;

internal sealed class GetUserDefaultPaymentMethodQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, IExecutionContextAccessor executionContext) : IQueryHandler<GetUserDefaultPaymentMethodQuery, PaymentMethod?>
{
    public async Task<PaymentMethod?> Handle(GetUserDefaultPaymentMethodQuery request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        var customerGetOptions = new CustomerGetOptions
        {
            Expand = new List<string> { "invoice_settings.default_payment_method" }
        };

        Customer customer = await stripeService.CustomerService.GetAsync(stripeCustomer.StripeCustomerId, customerGetOptions, cancellationToken: cancellationToken);
        if (customer is null || customer.InvoiceSettings is null || customer.InvoiceSettings.DefaultPaymentMethodId is null)
        {
            return null;
        }

        PaymentMethod paymentMethod = await stripeService.PaymentMethodService.GetAsync(customer.InvoiceSettings.DefaultPaymentMethodId, cancellationToken: cancellationToken) 
            ?? throw new ErrorException(PlansDomainErrors.Stripe.NoDefaultPaymentMethod);

        return paymentMethod;
    }
}
