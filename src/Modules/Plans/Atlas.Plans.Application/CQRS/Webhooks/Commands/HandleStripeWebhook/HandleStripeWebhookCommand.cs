using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Webhooks.Commands.HandleStripeWebhook;

public sealed record HandleStripeWebhookCommand(Event StripeEvent) : ICommand;
