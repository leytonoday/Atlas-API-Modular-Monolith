using Atlas.Shared.Domain.Events;

namespace Atlas.Users.Domain.Entities.UserEntity.Events;

public record UserUpdatedDomainEvent(Guid UserId) : IDomainEvent;
