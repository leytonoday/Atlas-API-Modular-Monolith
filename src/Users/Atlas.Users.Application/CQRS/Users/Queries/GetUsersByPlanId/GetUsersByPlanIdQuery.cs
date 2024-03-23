using Atlas.Users.Application.CQRS.Users.Shared;
using MediatR;

namespace Atlas.Infrastructure.CQRS.Users.Queries.GetUsersByPlanId;

public sealed record GetUsersByPlanIdQuery(Guid PlanId) : IRequest<IEnumerable<UserDto>>;
