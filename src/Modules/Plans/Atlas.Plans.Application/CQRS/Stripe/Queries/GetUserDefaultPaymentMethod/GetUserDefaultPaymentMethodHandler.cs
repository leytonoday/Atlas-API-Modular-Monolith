using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserDefaultPaymentMethod;

internal sealed class GetUserDefaultPaymentMethodQueryHandler(IPlansUnitOfWork unitOfWork, IStripeService stripeService, UserManager<User> userManager) : IRequestHandler<GetUserDefaultPaymentMethodQuery, PaymentMethod?>
{
    public async Task<PaymentMethod?> Handle(GetUserDefaultPaymentMethodQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await unitOfWork.StripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
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
