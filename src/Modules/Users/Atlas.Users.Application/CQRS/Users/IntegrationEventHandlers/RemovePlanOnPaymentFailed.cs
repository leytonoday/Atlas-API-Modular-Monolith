using Atlas.Plans.IntegrationEvents;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Users.Domain.Entities.UserEntity;

namespace Atlas.Users.Application.CQRS.Users.IntegrationEventHandlers;

public sealed class RemovePlanOnPaymentFailed(IUserRepository userRepository) : IIntegrationEventHandler<PaymentFailedIntegrationEvent>
{
    public async Task Handle(PaymentFailedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(notification.UserId, true, cancellationToken);
        if (user is null)
        {
            return;
        }

        User.SetPlanId(user, null);

        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
