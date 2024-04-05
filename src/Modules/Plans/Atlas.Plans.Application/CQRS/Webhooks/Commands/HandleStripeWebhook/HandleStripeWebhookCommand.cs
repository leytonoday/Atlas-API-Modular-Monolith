using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Webhooks.Commands.HandleStripeWebhook;

public sealed record HandleStripeWebhookCommand(Event StripeEvent) : ICommand;
