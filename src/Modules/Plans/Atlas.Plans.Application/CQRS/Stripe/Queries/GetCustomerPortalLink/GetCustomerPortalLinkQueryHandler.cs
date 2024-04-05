using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetCustomerPortalLink;

internal sealed class GetCustomerPortalLinkQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IExecutionContextAccessor executionContext, IStripeService stripeService) : IQueryHandler<GetCustomerPortalLinkQuery, string>
{
    public async Task<string> Handle(GetCustomerPortalLinkQuery request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        string url = await stripeService.CreateBillingPortalLinkAsync(stripeCustomer.StripeCustomerId, cancellationToken);
            
        return url;
    }
}
