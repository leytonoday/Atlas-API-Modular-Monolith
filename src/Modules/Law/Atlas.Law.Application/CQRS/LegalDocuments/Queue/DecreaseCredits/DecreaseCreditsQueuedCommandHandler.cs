using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Application.ModuleBridge;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queue.DecreaseCredits;

internal sealed class DecreaseCreditsQueuedCommandHandler(IModuleBridge moduleBridge) : IQueuedCommandHandler<DecreaseCreditsQueuedCommand>
{
    public async Task Handle(DecreaseCreditsQueuedCommand request, CancellationToken cancellationToken)
    {
        await moduleBridge.DecreaseUserCredits(request.UserId, cancellationToken);
    }
}