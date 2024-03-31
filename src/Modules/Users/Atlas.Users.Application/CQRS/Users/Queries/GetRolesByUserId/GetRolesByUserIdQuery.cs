using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Queries.GetRolesByUserId;

public sealed record GetRolesByUserIdQuery(Guid UserId) : IQuery<IEnumerable<string>>;
