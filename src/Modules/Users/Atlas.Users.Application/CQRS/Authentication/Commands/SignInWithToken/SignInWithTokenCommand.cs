using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignInWithToken;

public sealed record SignInWithTokenCommand(string Identifier, string Token) : ICommand;
