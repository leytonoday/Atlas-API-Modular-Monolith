using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetIsUserEligibleForTrial;

public sealed record GetIsUserEligibleForTrialQuery(Guid UserId) : IQuery<bool>;