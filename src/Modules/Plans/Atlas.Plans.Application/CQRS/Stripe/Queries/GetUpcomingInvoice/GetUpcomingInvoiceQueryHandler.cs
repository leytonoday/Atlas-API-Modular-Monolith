using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUpcomingInvoice;

internal sealed class GetUpcomingInvoiceQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, UserManager<User> userManager) : IQueryHandler<GetUpcomingInvoiceQuery, Invoice>
{
    public async Task<Invoice> Handle(GetUpcomingInvoiceQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
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
