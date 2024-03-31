using Atlas.Shared.IntegrationEvents;

namespace Atlas.Users.IntegrationEvents;

public sealed record UserEmailConfirmedIntegrationEvent(Guid UserId) : IntegrationEvent(Guid.NewGuid());
