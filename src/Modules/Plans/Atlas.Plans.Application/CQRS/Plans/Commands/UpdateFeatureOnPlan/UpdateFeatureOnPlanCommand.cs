using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.UpdateFeatureOnPlan;

public record UpdateFeatureOnPlanCommand(
    Guid PlanId,
    Guid FeatureId,
    string? Value
    ) : ICommand;
