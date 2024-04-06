using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserSubscription;

public sealed record GetUserSubscriptionQuery(Guid UserId) : IQuery<Subscription?>;