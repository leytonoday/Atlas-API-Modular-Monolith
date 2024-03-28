using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanFeaturesByPlanId;

public sealed record GetPlanFeaturesByPlanIdQuery(Guid PlanId) : IRequest<IEnumerable<PlanFeatureDto>>;
