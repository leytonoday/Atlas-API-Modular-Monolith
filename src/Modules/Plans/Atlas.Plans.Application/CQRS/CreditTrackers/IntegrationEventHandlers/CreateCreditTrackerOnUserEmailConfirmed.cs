using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Users.IntegrationEvents;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.IntegrationEventHandlers;

public sealed class CreateCreditTrackerOnUserEmailConfirmed(ICreditTrackerRepository creditTrackerRepository) : IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>
{
    public async Task Handle(UserEmailConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var creditTracker = CreditTracker.Create(notification.UserId, int.MinValue);
        await creditTrackerRepository.AddAsync(creditTracker, cancellationToken);
    }
}
