namespace Atlas.Shared.Domain.Events.UserEvents;

public sealed record UserEmailConfirmedEvent(Guid Id, Guid UserId) : DomainEvent(Id);