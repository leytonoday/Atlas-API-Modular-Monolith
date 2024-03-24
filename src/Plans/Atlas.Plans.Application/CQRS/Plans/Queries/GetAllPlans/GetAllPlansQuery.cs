using Atlas.Plans.Application.CQRS.Plans.Shared;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetAllPlans;

public sealed record GetAllPlansQuery(bool IncludeInactive) : IRequest<IEnumerable<PlanDto>>;
