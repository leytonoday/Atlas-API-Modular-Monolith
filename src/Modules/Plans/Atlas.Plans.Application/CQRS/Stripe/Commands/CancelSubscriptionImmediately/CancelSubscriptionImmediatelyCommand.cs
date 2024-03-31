using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscriptionImmediately;

public sealed record CancelSubscriptionImmediatelyCommand(Guid UserId) : ICommand;