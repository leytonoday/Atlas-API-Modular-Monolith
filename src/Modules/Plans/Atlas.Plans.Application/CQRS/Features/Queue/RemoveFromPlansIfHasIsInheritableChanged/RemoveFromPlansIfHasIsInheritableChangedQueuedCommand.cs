using Atlas.Shared.Application.Queue;

namespace Atlas.Plans.Application.CQRS.Features.RemoveFromPlansIfHasIsInheritableChanged.FeatureUpdated;

/// <summary>
/// 
/// </summary>
/// <param name="FeatureId"></param>
/// <param name="HasIsInheritableChanged">Determines if the "IsInheritable" boolean property was changed during the updating process.</param>
public sealed record RemoveFromPlansIfHasIsInheritableChangedQueuedCommand(Guid FeatureId, bool HasIsInheritableChanged) : IQueuedCommand;
