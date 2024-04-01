using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Users.Application.CQRS.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string UserName, string Email, string Password, string ConfirmPassword) : ICommand<string>; 