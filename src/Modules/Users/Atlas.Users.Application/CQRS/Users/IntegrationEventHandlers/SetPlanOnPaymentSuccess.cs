using Atlas.Plans.IntegrationEvents;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Users.Domain.Entities.UserEntity;

namespace Atlas.Users.Application.CQRS.Users.IntegrationEventHandlers;

public sealed class SetPlanOnPaymentSuccess(IUserRepository userRepository) : IIntegrationEventHandler<PaymentSuccessIntegrationEvent>
{
    public async Task Handle(PaymentSuccessIntegrationEvent notification, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(notification.UserId, true, cancellationToken);
        if (user is null)
        {
            return;
        }

        User.SetPlanId(user, notification.PlanId);

        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
