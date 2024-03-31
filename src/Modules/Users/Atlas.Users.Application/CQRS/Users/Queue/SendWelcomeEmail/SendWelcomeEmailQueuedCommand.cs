
using Atlas.Shared.Application.Queue;

namespace Atlas.Users.Application.CQRS.Users.Queue.SendWelcomeEmail;

public record SendWelcomeEmailQueuedCommand(Guid UserId) : IQueuedCommand;