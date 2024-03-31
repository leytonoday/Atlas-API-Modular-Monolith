using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscription;

public sealed record CancelSubscriptionCommand(Guid UserId) : ICommand;