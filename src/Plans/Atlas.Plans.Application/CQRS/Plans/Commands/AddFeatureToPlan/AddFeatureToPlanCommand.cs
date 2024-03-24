using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.AddFeatureToPlan;

public record AddFeatureToPlanCommand(
    Guid PlanId,
    Guid FeatureId,
    string? Value
    ) : IRequest;
