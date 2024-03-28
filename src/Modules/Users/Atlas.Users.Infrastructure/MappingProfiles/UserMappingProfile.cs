using Atlas.Users.Application.CQRS.Users.Shared;
using Atlas.Users.Domain.Entities.UserEntity;
using AutoMapper;

namespace Atlas.Users.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for the <see cref="User"/> entity and it's DTOs.
/// </summary>
public class UserMappingProfile : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="UserMappingProfile"/> class.
    /// </summary>
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}
