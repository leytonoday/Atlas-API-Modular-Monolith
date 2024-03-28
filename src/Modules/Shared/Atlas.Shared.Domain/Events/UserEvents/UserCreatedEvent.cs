namespace Atlas.Shared.Domain.Events.UserEvents;

public sealed record UserCreatedEvent(Guid Id, string Email) : DomainEvent(Id);