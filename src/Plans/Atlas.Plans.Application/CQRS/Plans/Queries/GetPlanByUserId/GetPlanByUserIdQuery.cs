using Atlas.Plans.Application.CQRS.Plans.Shared;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanByUserId;

public sealed record GetPlanByUserIdQuery(Guid UserId) : IRequest<PlanDto?>;
