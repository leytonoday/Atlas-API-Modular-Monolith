using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;

namespace Atlas.Plans.Infrastructure.CQRS.Features.Queries.GetAllFeatures;

public sealed record GetAllFeaturesQuery : IQuery<IEnumerable<FeatureDto>>;
