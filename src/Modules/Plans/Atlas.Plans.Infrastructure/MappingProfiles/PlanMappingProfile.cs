using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Plans.Domain.Entities.PlanEntity;
using AutoMapper;

namespace Atlas.Plans.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for the <see cref="Plan"/> entity and it's DTOs.
/// </summary>
public class PlanMappingProfile : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PlanMappingProfile"/> class.
    /// </summary>
    public PlanMappingProfile()
    {
        // Map from a PlanDto to a Plan
        // When mapping from a PlanDto to a Plan, ignore the Features and PlanFeatures properties. They will be updated seperately.
        CreateMap<PlanDto, Plan>()
            .ForMember(dest => dest.Features, opt => opt.Ignore())
            .ForMember(dest => dest.PlanFeatures, opt => opt.Ignore());

        // Map from a Plan to a PlanDto
        CreateMap<Plan, PlanDto>();
    }
}
