using Atlas.Plans.Domain.Entities.FeatureEntity;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.DeleteFeature;

public sealed record class DeleteFeatureCommand(Guid Id) : IRequest<Feature>;
