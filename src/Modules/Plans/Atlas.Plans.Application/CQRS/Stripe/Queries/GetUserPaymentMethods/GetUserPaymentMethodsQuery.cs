using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserPaymentMethodsQuery;

public sealed record GetUserPaymentMethodsQuery(Guid UserId) : IQuery<IEnumerable<PaymentMethod>>;