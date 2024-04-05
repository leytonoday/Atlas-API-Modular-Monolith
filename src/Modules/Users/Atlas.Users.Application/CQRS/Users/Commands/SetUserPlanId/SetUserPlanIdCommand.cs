using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Users.Application.CQRS.Users.Commands.SetUserPlanId;

public record SetUserPlanIdCommand(Guid UserId, Guid? PlanId) : ICommand;
