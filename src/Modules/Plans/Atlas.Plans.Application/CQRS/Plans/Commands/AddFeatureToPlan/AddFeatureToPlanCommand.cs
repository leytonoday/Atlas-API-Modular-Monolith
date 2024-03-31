using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.AddFeatureToPlan;

public record AddFeatureToPlanCommand(
    Guid PlanId,
    Guid FeatureId,
    string? Value
    ) : ICommand;
