using Atlas.Shared.IntegrationEvents;

namespace Atlas.Users.IntegrationEvents;

public sealed record UserUpdatedIntegrationEvent(Guid UserId, string UserName, string Email, string? PhoneNumber) : IntegrationEvent(Guid.NewGuid());