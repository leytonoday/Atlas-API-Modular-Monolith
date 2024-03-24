using Atlas.Plans.Domain.Entities.FeatureEntity;
using AutoMapper;

namespace Atlas.Plans.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for the <see cref="Feature"/> entity and it's DTOs.
/// </summary>
public class FeatureMappingProfile : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="FeatureMappingProfile"/> class.
    /// </summary>
    public FeatureMappingProfile()
    {
        CreateMap<Feature, FeatureDto>();
    }
}
