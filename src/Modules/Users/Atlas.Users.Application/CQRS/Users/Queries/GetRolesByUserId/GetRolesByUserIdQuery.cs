using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;

namespace Atlas.Infrastructure.CQRS.Users.Queries.GetRolesByUserId;

public sealed record GetRolesByUserIdQuery(Guid UserId) : IQuery<IEnumerable<string>>;
