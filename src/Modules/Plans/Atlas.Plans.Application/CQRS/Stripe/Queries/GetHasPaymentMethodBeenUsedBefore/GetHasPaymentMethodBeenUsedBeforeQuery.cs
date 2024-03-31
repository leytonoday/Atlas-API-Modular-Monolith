using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetHasPaymentMethodBeenUsedBefore;

public sealed record GetHasPaymentMethodBeenUsedBeforeQuery(string PaymentMethodId) : IQuery<bool>;