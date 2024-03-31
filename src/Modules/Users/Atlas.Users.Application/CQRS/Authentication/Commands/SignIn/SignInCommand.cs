using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignIn;

public sealed record SignInCommand(string Identifier, string Password) : ICommand;
