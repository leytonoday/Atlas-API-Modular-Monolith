using Atlas.Shared.IntegrationEvents;

namespace Atlas.Plans.IntegrationEvents;

public sealed record PaymentFailedIntegrationEvent(Guid UserId) : IntegrationEvent(Guid.NewGuid());
