using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserPaymentMethodsQuery;

internal sealed class GetUserPaymentMethodsQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, UserManager<User> userManager) : IRequestHandler<GetUserPaymentMethodsQuery, IEnumerable<PaymentMethod>>
{
    public async Task<IEnumerable<PaymentMethod>> Handle(GetUserPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
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
