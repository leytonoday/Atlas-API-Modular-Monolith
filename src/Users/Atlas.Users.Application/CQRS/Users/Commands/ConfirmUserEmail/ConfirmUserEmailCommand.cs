
using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.ConfirmUserEmail;

public sealed record ConfirmUserEmailCommand(string UserName, string Token): IRequest<string>;