using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanByUserId;

public sealed record GetPlanByUserIdQuery(Guid UserId) : IQuery<PlanDto?>;
