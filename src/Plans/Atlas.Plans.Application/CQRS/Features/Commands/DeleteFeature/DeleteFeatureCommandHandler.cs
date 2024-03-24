using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Features.Commands.DeleteFeature;

internal sealed class DeleteFeatureCommandHandler(IPlansUnitOfWork unitOfWork) : IRequestHandler<DeleteFeatureCommand, Feature>
{
    public async Task<Feature> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
    {
        Feature feature = await unitOfWork.FeatureRepository.GetByIdAsync(request.Id, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Feature.FeatureNotFound);

        await unitOfWork.FeatureRepository.RemoveAsync(feature, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return feature;
    }
}
