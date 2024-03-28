namespace Atlas.Shared.Domain.Events.UserEvents;

public sealed record UserUpdatedEvent(Guid Id, Guid UserId) : DomainEvent(Id);