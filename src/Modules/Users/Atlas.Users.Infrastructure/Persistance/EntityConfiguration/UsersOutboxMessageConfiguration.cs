using Atlas.Shared.Infrastructure.Persistance.Outbox;
using Atlas.Users.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Users.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="UsersOutboxMessage"/> entity.
/// </summary>
internal sealed class UsersOutboxMessageConfiguration : IEntityTypeConfiguration<UsersOutboxMessage>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="UsersOutboxMessage"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="UsersOutboxMessage"/> entity.</param>
    public void Configure(EntityTypeBuilder<UsersOutboxMessage> builder)
    {
        // Set the table name for this entity
        builder.ToTable(UsersConstants.TableNames.UsersOutboxMessages);

        // Set the primary key
        builder.HasKey(x => x.Id);
    }
}
