using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

public record CreatePlanCommand : ICommand<Guid>
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
}
