using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

public record CreatePlanCommand : ICommand<Guid>
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
}
