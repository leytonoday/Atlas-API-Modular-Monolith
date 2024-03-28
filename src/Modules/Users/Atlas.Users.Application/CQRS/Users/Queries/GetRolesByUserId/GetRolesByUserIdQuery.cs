using MediatR;

namespace Atlas.Infrastructure.CQRS.Users.Queries.GetRolesByUserId;

public sealed record GetRolesByUserIdQuery(Guid UserId) : IRequest<IEnumerable<string>>;
