using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.CreditTrackers.Commands.DecreaseUserCredits;

public sealed record DecreaseUserCreditsCommand(Guid UserId) : ICommand;
