using Atlas.Plans.Application.CQRS.Features.RemoveFromPlansIfHasIsInheritableChanged.FeatureUpdated;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Application.CQRS.Features.Commands.UpdateFeature;

internal sealed class UpdateFeatureCommandHandler(IFeatureRepository featureRepository, IQueueWriter queueWriter) : ICommandHandler<UpdateFeatureCommand, Feature>
{
    public async Task<Feature> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
    {
        Feature feature = await featureRepository.GetByIdAsync(request.Id, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Feature.FeatureNotFound);

        bool hasIsInheritableChanged = request.IsInheritable != feature.IsInheritable;

        await Feature.UpdateAsync(
            feature,
            request.Name, 
            request.Description, 
            request.IsInheritable, 
            request.IsHidden,
            featureRepository, 
            cancellationToken);

        await featureRepository.UpdateAsync(feature, cancellationToken);

        await queueWriter.WriteAsync(new RemoveFromPlansIfHasIsInheritableChangedQueuedCommand(feature.Id, hasIsInheritableChanged), cancellationToken);

        return feature;
    }
}
