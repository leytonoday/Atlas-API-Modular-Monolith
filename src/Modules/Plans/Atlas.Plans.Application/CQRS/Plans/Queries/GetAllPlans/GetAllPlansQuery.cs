using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetAllPlans;

public sealed record GetAllPlansQuery(bool IncludeInactive) : IQuery<IEnumerable<PlanDto>>;
