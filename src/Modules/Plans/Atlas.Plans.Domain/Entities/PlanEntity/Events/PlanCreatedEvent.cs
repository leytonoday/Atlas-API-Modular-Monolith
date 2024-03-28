using Atlas.Shared.Domain.Events;

namespace Atlas.Plans.Domain.Entities.PlanEntity.Events;

public sealed record PlanCreatedEvent(Guid Id, string PlanName) : DomainEvent(Id);