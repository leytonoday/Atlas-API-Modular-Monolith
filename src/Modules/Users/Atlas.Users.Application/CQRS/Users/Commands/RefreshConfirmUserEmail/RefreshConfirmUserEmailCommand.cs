using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.RefreshConfirmUserEmail;

public sealed record RefreshConfirmUserEmailCommand(string Identifier) : ICommand;