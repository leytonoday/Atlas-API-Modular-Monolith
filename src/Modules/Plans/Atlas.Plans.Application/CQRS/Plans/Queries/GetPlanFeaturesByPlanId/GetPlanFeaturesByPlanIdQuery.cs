using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanFeaturesByPlanId;

public sealed record GetPlanFeaturesByPlanIdQuery(Guid PlanId) : IQuery<IEnumerable<PlanFeatureDto>>;
