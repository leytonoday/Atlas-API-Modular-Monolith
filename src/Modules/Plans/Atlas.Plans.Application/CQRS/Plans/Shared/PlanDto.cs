using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;

namespace Atlas.Plans.Application.CQRS.Plans.Shared;

public class PlanDto : BasePlanDto
{
    public Guid Id { get; set; }

    public IEnumerable<PlanFeatureDto>? PlanFeatures { get; set; }

    public IEnumerable<FeatureDto>? Features { get; set; }

    public IEnumerable<PlanFeatureDto>? InheritedPlanFeatures { get; set; }

    public IEnumerable<FeatureDto>? InheritedFeatures { get; set; }
}
