using Atlas.Shared.IntegrationEvents;

namespace Atlas.Users.IntegrationEvents;

public record class UserEmailConfirmedIntegrationEvent(Guid UserId) : IntegrationEvent(Guid.NewGuid());
