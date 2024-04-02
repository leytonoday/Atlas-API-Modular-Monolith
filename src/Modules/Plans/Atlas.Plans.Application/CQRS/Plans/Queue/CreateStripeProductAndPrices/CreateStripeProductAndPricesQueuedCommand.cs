using Atlas.Shared.Application.Queue;

namespace Atlas.Plans.Application.CQRS.Plans.Queue.CreateStripeProductAndPrices;

public sealed record CreateStripeProductAndPricesQueuedCommand(string PlanName) : IQueuedCommand;
