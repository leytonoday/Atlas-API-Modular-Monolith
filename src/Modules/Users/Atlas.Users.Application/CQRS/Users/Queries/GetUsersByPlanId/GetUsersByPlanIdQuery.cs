using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Users.Shared;
using MediatR;

namespace Atlas.Users.Application.CQRS.Users.Queries.GetUsersByPlanId;

public sealed record GetUsersByPlanIdQuery(Guid PlanId) : IQuery<IEnumerable<UserDto>>;
