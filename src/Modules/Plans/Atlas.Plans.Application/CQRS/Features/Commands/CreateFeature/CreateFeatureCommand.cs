using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.CreateFeature;

public sealed record class CreateFeatureCommand(string Name, string Description, bool IsInheritable, bool IsHidden) : ICommand<Feature>;
