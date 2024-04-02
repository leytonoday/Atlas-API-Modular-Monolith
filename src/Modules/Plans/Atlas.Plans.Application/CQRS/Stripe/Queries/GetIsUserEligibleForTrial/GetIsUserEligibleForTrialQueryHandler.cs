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
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetIsUserEligibleForTrial;

internal sealed class GetIsUserEligibleForTrialQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, IExecutionContextAccessor executionContext) : IQueryHandler<GetIsUserEligibleForTrialQuery, bool>
{
    public async Task<bool> Handle(GetIsUserEligibleForTrialQuery request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        return await stripeService.IsUserEligibleForTrialAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }
}
