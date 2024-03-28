using MediatR;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscriptionImmediately;

public sealed record CancelSubscriptionImmediatelyCommand(Guid UserId) : IRequest;