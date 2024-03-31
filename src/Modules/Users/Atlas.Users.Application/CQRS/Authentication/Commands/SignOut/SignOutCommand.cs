using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignOut;

public sealed record SignOutCommand : ICommand;
