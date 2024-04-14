using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.Commands.DecreaseUserCredits;

internal sealed class DecreaseUserCreditsCommandHandler(ICreditTrackerRepository creditTrackerRepository) : ICommandHandler<DecreaseUserCreditsCommand>
{
    public async Task Handle(DecreaseUserCreditsCommand request, CancellationToken cancellationToken)
    {
        CreditTracker creditTracker = await creditTrackerRepository.GetByUserIdAsync(request.UserId, true, cancellationToken);
        CreditTracker.DecreaseCurrentCreditCount(creditTracker);
    }
}
