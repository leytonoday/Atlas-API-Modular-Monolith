using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.UpdateFeature;

internal sealed class UpdateFeatureCommandHandler(IPlansUnitOfWork unitOfWork) : IRequestHandler<UpdateFeatureCommand, Feature>
{
    public async Task<Feature> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await Feature.UpdateAsync(
            request.Id,
            request.Name, 
            request.Description, 
            request.IsInheritable, 
            request.IsHidden,
            unitOfWork.FeatureRepository, 
            cancellationToken);

        await unitOfWork.FeatureRepository.UpdateAsync(feature, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return feature;
    }
}
