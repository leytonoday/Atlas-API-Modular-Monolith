using Atlas.Shared.Domain.Events;

namespace Atlas.Plans.Domain.Entities.FeatureEntity.DomainEvents;

public record FeatureUpdatedDomainEvent(Guid FeatureId, bool HasIsInheritableChanged) : IDomainEvent;
