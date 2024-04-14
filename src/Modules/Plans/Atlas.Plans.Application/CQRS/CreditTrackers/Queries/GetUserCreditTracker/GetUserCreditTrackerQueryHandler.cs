using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using AutoMapper;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetUserCreditTracker;

internal sealed class GetUserCreditTrackerQueryHandler(ICreditTrackerRepository creditTrackerRepository, IMapper mapper) : IQueryHandler<GetUserCreditTrackerQuery, CreditTrackerDto?>
{
    public async Task<CreditTrackerDto?> Handle(GetUserCreditTrackerQuery request, CancellationToken cancellationToken)
    {
        CreditTracker? creditTracker = await creditTrackerRepository.GetByUserIdAsync(request.UserId, false, cancellationToken);
        if (creditTracker is null)
        {
            return null;
        }

        return mapper.Map<CreditTrackerDto>(creditTracker);
    }
}
