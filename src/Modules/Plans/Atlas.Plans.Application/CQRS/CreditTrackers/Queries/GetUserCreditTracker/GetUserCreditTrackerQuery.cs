using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetUserCreditTracker;

public sealed record GetUserCreditTrackerQuery(Guid UserId) : IQuery<CreditTrackerDto?>;
