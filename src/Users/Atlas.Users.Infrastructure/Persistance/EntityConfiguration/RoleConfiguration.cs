using Atlas.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Users.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="IdentityRole"/> entity.
/// </summary>
internal sealed class RoleConfiguration: IEntityTypeConfiguration<IdentityRole<Guid>>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="IdentityRole"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="IdentityRole"/> entity.</param>
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        // Set the table name for this entity
        builder.ToTable(UsersConstants.TableNames.Roles);

        // Add database level constraint to ensure the role name is unique
        builder.HasIndex(x => x.Name).IsUnique();

        // Add database level constraint to ensure the normalised role name is unique
        builder.HasIndex(x => x.NormalizedName).IsUnique();

        // Add default roles
        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        // Make the default support user.
        var user = new IdentityRole<Guid>
        {
            Id = UsersConstants.SeedData.AdministratorRoleId,
            Name = RoleNames.Administrator,
            NormalizedName = RoleNames.Administrator.ToUpperInvariant(),
        };

        builder.HasData(user);
    }
}
