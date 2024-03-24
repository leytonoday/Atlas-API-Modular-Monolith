using Atlas.Plans.Application.CQRS.Plans.Shared;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanById;

public sealed record GetPlanByIdQuery(Guid Id) : IRequest<PlanDto>;
