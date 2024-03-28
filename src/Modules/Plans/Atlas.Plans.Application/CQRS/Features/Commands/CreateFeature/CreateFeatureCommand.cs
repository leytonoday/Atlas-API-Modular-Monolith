using Atlas.Plans.Domain.Entities.FeatureEntity;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.CreateFeature;

public sealed record class CreateFeatureCommand(string Name, string Description, bool IsInheritable, bool IsHidden) : IRequest<Feature>;
