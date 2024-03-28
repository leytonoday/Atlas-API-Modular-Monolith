using Atlas.Shared.Domain.Events;

namespace Atlas.Plans.Domain.Entities.FeatureEntity.Events;

/// <summary>
/// An event that indicates that the <see cref="Feature"/> with Id equal to <paramref name="Id"/> has been updated.
/// </summary>
/// <param name="Id">The Id of the <see cref="Feature"/> that has been updated.</param>
/// <param name="HasIsInheritableChanged">Determines if the "IsInheritable" boolean property was changed during the updating process.</param>
public sealed record FeatureUpdatedEvent(Guid Id, bool HasIsInheritableChanged) : DomainEvent(Id);