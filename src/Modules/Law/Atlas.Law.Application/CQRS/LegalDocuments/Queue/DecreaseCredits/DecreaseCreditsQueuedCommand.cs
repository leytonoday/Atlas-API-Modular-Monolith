using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queue.DecreaseCredits;

public sealed record DecreaseCreditsQueuedCommand(Guid UserId) : QueuedCommand;
