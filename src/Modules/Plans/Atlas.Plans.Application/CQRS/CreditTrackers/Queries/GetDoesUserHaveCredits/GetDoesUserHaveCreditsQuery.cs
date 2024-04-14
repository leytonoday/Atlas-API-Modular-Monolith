using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetDoesUserHaveCredits;

public sealed record GetDoesUserHaveCreditsQuery(Guid UserId) : IQuery<bool>;
