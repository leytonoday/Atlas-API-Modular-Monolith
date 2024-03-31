using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.DeleteFeature;

public sealed record class DeleteFeatureCommand(Guid Id) : ICommand<Feature>;
