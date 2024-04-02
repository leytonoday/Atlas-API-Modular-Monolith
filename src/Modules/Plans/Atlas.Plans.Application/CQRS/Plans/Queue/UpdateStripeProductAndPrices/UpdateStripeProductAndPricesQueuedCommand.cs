using Atlas.Shared.Application.Queue;

namespace Atlas.Plans.Application.CQRS.Plans.Queue.UpdateStripeProductAndPrices;

public sealed record UpdateStripeProductAndPricesQueuedCommand(Guid PlanId, bool HasBeenDeactivated, bool HasBeenReactivated, bool HavePricesChanged) : IQueuedCommand;
