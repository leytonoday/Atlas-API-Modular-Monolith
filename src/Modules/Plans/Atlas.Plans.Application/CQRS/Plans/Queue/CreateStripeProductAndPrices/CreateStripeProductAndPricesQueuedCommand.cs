using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Application.Queue;

namespace Atlas.Plans.Application.CQRS.Plans.Queue.CreateStripeProductAndPrices;

public sealed record CreateStripeProductAndPricesQueuedCommand(string PlanName) : QueuedCommand;
