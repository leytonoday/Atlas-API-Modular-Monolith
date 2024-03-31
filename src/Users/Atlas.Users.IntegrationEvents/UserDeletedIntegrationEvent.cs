using Atlas.Shared.IntegrationEvents;

namespace Atlas.Users.IntegrationEvents;

public sealed record UserDeletedIntegrationEvent(Guid UserId) : IntegrationEvent(Guid.NewGuid());
