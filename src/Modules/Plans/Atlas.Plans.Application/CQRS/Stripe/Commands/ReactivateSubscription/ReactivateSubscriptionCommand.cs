using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.ReactivateSubscription;

public sealed record ReactivateSubscriptionCommand(Guid UserId) : IRequest;