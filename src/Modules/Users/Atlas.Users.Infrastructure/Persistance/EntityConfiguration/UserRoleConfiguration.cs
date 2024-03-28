using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Users.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="IdentityUserRole<Guid>"/> entity.
/// </summary>
internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="IdentityUserRole<Guid>"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="IdentityUserRole<Guid>"/> entity.</param>
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        // Set the table name for this entity
        builder.ToTable(UsersConstants.TableNames.UserRoles);

        // Seed data
        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        // Add values for the default support user
        var userRole = new IdentityUserRole<Guid>()
        {
            RoleId = UsersConstants.SeedData.AdministratorRoleId, 
            UserId = UsersConstants.SeedData.SupportUserId 
        };

        builder.HasData(userRole);
    }
}
