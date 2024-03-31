using Atlas.Shared.IntegrationEvents;

namespace Atlas.Users.IntegrationEvents;

public sealed record UserUpdatedIntegrationEvent(Guid UserId) : IntegrationEvent(Guid.NewGuid());