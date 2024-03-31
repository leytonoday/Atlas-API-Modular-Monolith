using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId, string Password) : ICommand;
