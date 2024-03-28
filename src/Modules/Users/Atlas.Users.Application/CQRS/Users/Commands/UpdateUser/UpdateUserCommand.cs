using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(string UserName, string? PhoneNumber) :IRequest;
