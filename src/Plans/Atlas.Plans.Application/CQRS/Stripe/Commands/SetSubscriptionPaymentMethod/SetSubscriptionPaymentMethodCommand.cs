using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.SetSubscriptionPaymentMethod;

public sealed record SetSubscriptionPaymentMethodCommand(string StripePaymentMethodId, Guid UserId) : IRequest;