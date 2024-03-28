using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetSubscriptionQuoteInvoice;

public sealed record GetSubscriptionQuoteInvoiceQuery(Guid UserId, Guid PlanId, string Interval, string? PromotionCode) : IRequest<Invoice>;