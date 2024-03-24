namespace Atlas.Plans.Infrastructure.CQRS.PlanFeatures;

public sealed class PlanFeatureDto
{
    public Guid PlanId { get; set; }
    public Guid FeatureId { get; set; }
    public string? Value { get; set; }
    public bool? IsInherited { get; set; }
}
