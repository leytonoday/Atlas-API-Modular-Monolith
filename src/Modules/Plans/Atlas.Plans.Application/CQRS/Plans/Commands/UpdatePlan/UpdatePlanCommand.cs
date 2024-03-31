using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

public record UpdatePlanCommand : ICommand
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string IsoCurrencyCode { get; set; } = null!;
    public int MonthlyPrice { get; set; }
    public int AnnualPrice { get; set; }
    public int TrialPeriodDays { get; set; }
    public string? Tag { get; set; }
    public string Icon { get; set; } = null!;
    public string IconColour { get; set; } = null!;
    public string? BackgroundColour { get; set; }
    public string? TextColour { get; set; }
    public bool Active { get; set; }
    public Guid? InheritsFromId { get; set; }

    public Guid Id { get; set; }

    public IEnumerable<PlanFeatureDto>? PlanFeatures { get; set; }

    public IEnumerable<FeatureDto>? Features { get; set; }

    public IEnumerable<PlanFeatureDto>? InheritedPlanFeatures { get; set; }

    public IEnumerable<FeatureDto>? InheritedFeatures { get; set; }
}