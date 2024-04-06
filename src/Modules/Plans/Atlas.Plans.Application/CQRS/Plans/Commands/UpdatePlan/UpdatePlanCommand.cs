using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

public record UpdatePlanCommand : ICommand
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string IsoCurrencyCode { get; init; } = null!;
    public int MonthlyPrice { get; init; }
    public int AnnualPrice { get; init; }
    public int TrialPeriodDays { get; init; }
    public string? Tag { get; init; }
    public string Icon { get; init; } = null!;
    public string IconColour { get; init; } = null!;
    public string? BackgroundColour { get; init; }
    public string? TextColour { get; init; }
    public bool Active { get; init; }
    public Guid? InheritsFromId { get; init; }

    public Guid Id { get; init; }

    public IEnumerable<PlanFeatureDto>? PlanFeatures { get; init; }

    public IEnumerable<FeatureDto>? Features { get; init; }

    public IEnumerable<PlanFeatureDto>? InheritedPlanFeatures { get; init; }

    public IEnumerable<FeatureDto>? InheritedFeatures { get; init; }
}