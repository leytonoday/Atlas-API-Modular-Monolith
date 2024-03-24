using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.CreateFeature;

internal sealed class CreateFeatureCommandHandler(IPlansUnitOfWork unitOfWork) : IRequestHandler<CreateFeatureCommand, Feature>
{
    public async Task<Feature> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await Feature.CreateAsync(
            request.Name, 
            request.Description, 
            request.IsInheritable, 
            request.IsHidden,
            unitOfWork.FeatureRepository, 
            cancellationToken);

        await unitOfWork.FeatureRepository.AddAsync(feature, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return feature;
    }
}
