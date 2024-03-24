using Atlas.Users.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Users.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="User"/> entity.
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="User"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="User"/> entity.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Set the table name for this entity
        builder.ToTable(UsersConstants.TableNames.Users);

        // Set the primary key
        builder.HasKey(x => x.Id);

        // Add database level constraint to ensure username is unique
        builder.HasIndex(x => x.UserName).IsUnique();

        // Add database level constraint to ensure email is unique
        builder.HasIndex(x => x.Email).IsUnique();

        // Seed data
        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<User> builder)
    {
        // Make the default support user.
        var user = new User()
        {
            Id = UsersConstants.SeedData.SupportUserId,
            Email = UsersConstants.SeedData.SupportUserEmail,
            NormalizedEmail = UsersConstants.SeedData.SupportUserEmail.ToUpperInvariant(),
            UserName = UsersConstants.SeedData.SupportUserUserName,
            NormalizedUserName = UsersConstants.SeedData.SupportUserUserName.ToUpperInvariant(),
            SecurityStamp = new Guid("009e1404-ca20-43b8-a1f3-85371b12cf28").ToString(),
            ConcurrencyStamp = new Guid("33aa46c4-830b-4d60-9543-34276d5525a2").ToString(),
            EmailConfirmed = true,
            CreatedOnUtc = new DateTime(2023, 7, 22, 1, 36, 36, 564, DateTimeKind.Unspecified).AddTicks(0), // Must be specified directly. See https://github.com/dotnet/efcore/issues/12346
            PasswordHash = "AQAAAAIAAYagAAAAEFqB4xPd70Vo/tTAV3CHqe4BXXfDt+iYaTjkuZA7SB+FBqo2al/PkOIJ22v7DEq0jA==" // Hash for 'Password12!'
        };

        builder.HasData(user);
    }
}
