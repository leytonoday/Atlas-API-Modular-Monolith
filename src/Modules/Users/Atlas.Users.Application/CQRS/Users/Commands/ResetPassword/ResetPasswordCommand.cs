using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string UserName, string NewPassword, string Token) : IRequest;