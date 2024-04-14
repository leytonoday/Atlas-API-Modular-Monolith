using Atlas.Shared.IntegrationEvents;

namespace Atlas.Plans.IntegrationEvents;

public sealed record PaymentSuccessIntegrationEvent(Guid UserId, Guid PlanId) : IntegrationEvent(Guid.NewGuid());
