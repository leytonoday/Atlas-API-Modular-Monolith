
using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Users.Application.CQRS.Users.Queue.SendWelcomeEmail;

public record SendWelcomeEmailQueuedCommand(Guid UserId) : QueuedCommand(Guid.NewGuid());