namespace Atlas.Shared.Domain.Events.UserEvents;

public sealed record UserDeletedEvent(Guid Id, Guid UserId) : DomainEvent(Id);