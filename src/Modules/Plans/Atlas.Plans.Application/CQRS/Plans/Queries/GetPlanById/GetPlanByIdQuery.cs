using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanById;

public sealed record GetPlanByIdQuery(Guid Id) : IQuery<PlanDto>;
