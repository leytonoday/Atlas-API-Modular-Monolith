using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Features.Shared;

public sealed class FeatureDto : BaseDto<Guid>
{
    public string Name { get; set;  }
    public string? Description { get; set; }
    public bool IsInheritable { get; set; }
    public bool IsHidden { get; set; }

}
