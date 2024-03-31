using Atlas.Plans.Application.CQRS.Stripe.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetInvoiceHistory;

public sealed record GetInvoiceHistoryQuery(Guid UserId, int? Limit, string? StartingAfter) : IQuery<IEnumerable<StripeSlimInvoiceDto>>;