using Atlas.Users.Application.CQRS.Users.Shared;
using MediatR;

namespace Atlas.Infrastructure.CQRS.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;