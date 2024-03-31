using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.UpdateFeature;

public sealed record class UpdateFeatureCommand(Guid Id, string Name, string Description, bool IsInheritable, bool IsHidden) : ICommand<Feature>;
