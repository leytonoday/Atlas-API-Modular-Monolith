using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetHasPaymentMethodBeenUsedBefore;

public sealed record GetHasPaymentMethodBeenUsedBeforeQuery(string PaymentMethodId) : IRequest<bool>;