using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.SetSubscriptionPaymentMethod;

public sealed record SetSubscriptionPaymentMethodCommand(string StripePaymentMethodId, Guid UserId) : ICommand;