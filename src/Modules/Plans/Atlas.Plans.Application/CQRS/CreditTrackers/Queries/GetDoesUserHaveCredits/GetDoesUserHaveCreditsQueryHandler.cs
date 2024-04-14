using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetDoesUserHaveCredits;

internal class GetDoesUserHaveCreditsQueryHandler(ICreditTrackerRepository creditTrackerRepository) : IQueryHandler<GetDoesUserHaveCreditsQuery, bool>
{
    public async Task<bool> Handle(GetDoesUserHaveCreditsQuery request, CancellationToken cancellationToken)
    {
        CreditTracker creditTracker = await creditTrackerRepository.GetByUserIdAsync(request.UserId, false, cancellationToken);

        return creditTracker.CurrentCreditCount > 0;
    }
}
