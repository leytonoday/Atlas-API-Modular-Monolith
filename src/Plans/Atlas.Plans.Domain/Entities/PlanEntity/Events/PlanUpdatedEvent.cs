using Atlas.Shared.Domain.Events;

namespace Atlas.Plans.Domain.Entities.PlanEntity.Events;

public sealed record PlanUpdatedEvent(Guid Id, Guid PlanId, bool HasBeenDeactivated, bool HasBeenReactivated, bool HavePricesChanged) : DomainEvent(Id);