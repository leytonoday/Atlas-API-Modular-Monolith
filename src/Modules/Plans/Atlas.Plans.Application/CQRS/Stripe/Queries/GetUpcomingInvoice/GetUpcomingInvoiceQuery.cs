using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUpcomingInvoice;

public sealed record GetUpcomingInvoiceQuery(Guid UserId) : IQuery<Invoice>;