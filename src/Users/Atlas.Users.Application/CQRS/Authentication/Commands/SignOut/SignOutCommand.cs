using MediatR;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignOut;

public sealed record SignOutCommand : IRequest;
