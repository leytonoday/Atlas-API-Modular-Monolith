using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

public record UpdatePlanCommand : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string IsoCurrencyCode { get; set; }
    public int MonthlyPrice { get; set; }
    public int AnnualPrice { get; set; }
    public int TrialPeriodDays { get; set; }
    public string? Tag { get; set; }
    public string Icon { get; set; }
    public string IconColour { get; set; }
    public string BackgroundColour { get; set; }
    public string TextColour { get; set; }
    public bool Active { get; set; }
    public Guid? InheritsFromId { get; set; }

    public Guid Id { get; set; }

    public IEnumerable<PlanFeatureDto>? PlanFeatures { get; set; }

    public IEnumerable<FeatureDto>? Features { get; set; }

    public IEnumerable<PlanFeatureDto>? InheritedPlanFeatures { get; set; }

    public IEnumerable<FeatureDto>? InheritedFeatures { get; set; }
}