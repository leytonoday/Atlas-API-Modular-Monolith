using Atlas.Plans.Domain.Services;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetIsPromotionCodeValid;

internal sealed class GetIsPromotionCodeValidQueryHandler(IStripeService stripeService) : IRequestHandler<GetIsPromotionCodeValidQuery, bool>
{
    public async Task<bool> Handle(GetIsPromotionCodeValidQuery request, CancellationToken cancellationToken)
    {
        Coupon? coupon = await stripeService.GetCouponFromPromotionCodeAsync(request.PromotionCode, cancellationToken);
        return coupon is not null; // If it's null, then it's invalid. Otherwise, it's valid
    }
}
