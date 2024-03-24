using Atlas.Plans.Application.CQRS.Features.Shared;
using MediatR;

namespace Atlas.Plans.Infrastructure.CQRS.Features.Queries.GetAllFeatures;

public sealed record GetAllFeaturesQuery : IRequest<IEnumerable<FeatureDto>>;
