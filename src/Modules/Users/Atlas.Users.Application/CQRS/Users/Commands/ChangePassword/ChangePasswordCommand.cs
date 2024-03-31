using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(string OldPassword, string NewPassword) : ICommand;