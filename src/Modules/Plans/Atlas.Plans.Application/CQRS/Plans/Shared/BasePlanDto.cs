using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Shared;

public class BasePlanDto
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
}
