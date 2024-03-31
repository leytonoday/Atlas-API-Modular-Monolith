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

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetIsUserEligibleForTrial;

internal sealed class GetIsUserEligibleForTrialQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, UserManager<User> userManager) : IRequestHandler<GetIsUserEligibleForTrialQuery, bool>
{
    public async Task<bool> Handle(GetIsUserEligibleForTrialQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        return await stripeService.IsUserEligibleForTrialAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }
}
