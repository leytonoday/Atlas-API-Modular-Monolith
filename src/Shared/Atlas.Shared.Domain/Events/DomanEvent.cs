namespace Atlas.Shared.Domain.Events;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
