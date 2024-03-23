using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Identifier) : IRequest;