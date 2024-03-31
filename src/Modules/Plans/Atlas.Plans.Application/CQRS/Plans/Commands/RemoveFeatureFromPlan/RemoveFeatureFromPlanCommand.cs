using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeature;

public record RemoveFeatureFromPlanCommand(
    Guid PlanId,
    Guid FeatureId
    ) : ICommand;
