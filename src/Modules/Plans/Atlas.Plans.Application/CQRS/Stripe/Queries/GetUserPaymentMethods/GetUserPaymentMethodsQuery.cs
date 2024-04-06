using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserPaymentMethodsQuery;

public sealed record GetUserPaymentMethodsQuery(Guid UserId) : IQuery<IEnumerable<PaymentMethod>>;