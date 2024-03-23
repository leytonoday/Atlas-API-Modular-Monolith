using MediatR;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignInWithToken;

public sealed record SignInWithTokenCommand(string Identifier, string Token) : IRequest;
