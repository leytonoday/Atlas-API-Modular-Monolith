using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserDefaultPaymentMethod;

public sealed record GetUserDefaultPaymentMethodQuery(Guid UserId) : IQuery<PaymentMethod?>;