using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Users.IntegrationEvents;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.IntegrationEventHandlers;

public sealed class DeleteCreditTrackerOnUserDeleted(ICreditTrackerRepository creditTrackerRepository) : IIntegrationEventHandler<UserDeletedIntegrationEvent>
{
    public async Task Handle(UserDeletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        CreditTracker? creditTracker = (await creditTrackerRepository.GetByConditionAsync(x => x.UserId == notification.UserId, true, cancellationToken)).FirstOrDefault();
        if (creditTracker is null)
        {
            return;
        }

        await creditTrackerRepository.RemoveAsync(creditTracker, cancellationToken);
    }
}
