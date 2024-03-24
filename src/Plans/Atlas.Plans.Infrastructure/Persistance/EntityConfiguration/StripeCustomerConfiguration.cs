using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="StripeCustomer"/> entity.
/// </summary>
internal sealed class StripeCustomerConfiguration : IEntityTypeConfiguration<StripeCustomer>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="StripeCustomer"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="StripeCustomer"/> entity.</param>
    public void Configure(EntityTypeBuilder<StripeCustomer> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansTableNames.StripeCustomers);

        // Set the composite primary key
        builder.HasKey(x => new { x.UserId, x.StripeCustomerId });
    }
}
