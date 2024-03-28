using Atlas.Plans.Domain.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetPublishableKey;

internal sealed class GetPublishableKeyQueryHandler(IStripeService stripeService) : IRequestHandler<GetPublishableKeyQuery, string>
{
    public Task<string> Handle(GetPublishableKeyQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(stripeService.GetPublishableKey());
    }
}
