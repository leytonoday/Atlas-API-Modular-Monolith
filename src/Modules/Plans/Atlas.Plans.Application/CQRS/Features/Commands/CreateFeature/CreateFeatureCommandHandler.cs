using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.CreateFeature;

internal sealed class CreateFeatureCommandHandler(IFeatureRepository featureRepository) : ICommandHandler<CreateFeatureCommand, Feature>
{
    public async Task<Feature> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await Feature.CreateAsync(
            request.Name, 
            request.Description, 
            request.IsInheritable, 
            request.IsHidden,
            featureRepository, 
            cancellationToken);

        await featureRepository.AddAsync(feature, cancellationToken);
        return feature;
    }
}
