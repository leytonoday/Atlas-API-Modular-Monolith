using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.DeleteFeature;

internal sealed class DeleteFeatureCommandHandler(IFeatureRepository featureRepository) : ICommandHandler<DeleteFeatureCommand, Feature>
{
    public async Task<Feature> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
    {
        Feature feature = await featureRepository.GetByIdAsync(request.Id, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Feature.FeatureNotFound);

        await featureRepository.RemoveAsync(feature, cancellationToken);
        return feature;
    }
}
