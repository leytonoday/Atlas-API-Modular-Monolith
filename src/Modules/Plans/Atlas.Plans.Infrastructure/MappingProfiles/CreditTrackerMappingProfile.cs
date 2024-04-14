using Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetUserCreditTracker;
using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using AutoMapper;

namespace Atlas.Plans.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for the <see cref="CreditTracker"/> entity and it's DTOs.
/// </summary>
public class CreditTrackerMappingProfile : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="CreditTrackerMappingProfile"/> class.
    /// </summary>
    public CreditTrackerMappingProfile()
    {
        // Map from a Plan to a PlanDto
        CreateMap<CreditTracker, CreditTrackerDto>();
    }
}
