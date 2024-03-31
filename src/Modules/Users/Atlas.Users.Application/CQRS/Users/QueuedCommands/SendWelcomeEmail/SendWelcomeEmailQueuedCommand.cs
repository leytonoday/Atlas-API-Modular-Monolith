
using Atlas.Shared.Application.Queue;

namespace Atlas.Users.Application.CQRS.Users.QueuedCommands.SendWelcomeEmail;

public record SendWelcomeEmailQueuedCommand(Guid UserId) : IQueuedCommand;