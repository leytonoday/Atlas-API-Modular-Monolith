using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using AutoMapper;

namespace Atlas.Plans.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for the <see cref="PlanFeature"/> entity and it's DTOs.
/// </summary>
public class PlanFeatureMappingProfile : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PlanFeatureMappingProfile"/> class.
    /// </summary>
    public PlanFeatureMappingProfile()
    {
        CreateMap<PlanFeature, PlanFeatureDto>();
        CreateMap<PlanFeatureDto, PlanFeature>();
    }
}
