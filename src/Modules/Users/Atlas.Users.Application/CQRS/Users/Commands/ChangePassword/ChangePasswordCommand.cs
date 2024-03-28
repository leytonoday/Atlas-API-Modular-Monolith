using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest;