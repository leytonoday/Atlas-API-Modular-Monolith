using MediatR;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetIsPromotionCodeValid;

public sealed record GetIsPromotionCodeValidQuery(string PromotionCode) : IRequest<bool>;