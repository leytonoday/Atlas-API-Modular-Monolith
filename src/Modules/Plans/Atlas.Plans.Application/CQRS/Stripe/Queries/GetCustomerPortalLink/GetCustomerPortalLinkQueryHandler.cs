using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetCustomerPortalLink;

internal sealed class GetCustomerPortalLinkQueryHandler(IStripeCustomerRepository stripeCustomerRepository, UserManager<User> userManager, IStripeService stripeService) : IQueryHandler<GetCustomerPortalLinkQuery, string>
{
    public async Task<string> Handle(GetCustomerPortalLinkQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        string url = await stripeService.CreateBillingPortalLinkAsync(stripeCustomer.StripeCustomerId, cancellationToken);
            
        return url;
    }
}
