using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.CancelSubscription;

public sealed record GetUserSubscriptionQuery(Guid UserId) : IRequest<Subscription?>;