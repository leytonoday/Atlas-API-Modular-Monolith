using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Atlas.Shared;
using Atlas.Shared.Infrastructure.Options;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Atlas.Plans.Infrastructure.Persistance;

public class PlansDatabaseContextFactory : IDesignTimeDbContextFactory<PlansDatabaseContext>
{
    public PlansDatabaseContext CreateDbContext(string[] args)
    {
        string environment = Utils.IsDevelopment() ? "Development" : "Production";

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{environment}.json")
            .Build();

        // Create DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<PlansDatabaseContext>();

        // Configure DbContext options
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Atlas"), options =>
        {
            options.MigrationsAssembly(typeof(PlansDatabaseContext).Assembly.GetName().Name);
            options.MigrationsHistoryTable(SqlServerHistoryRepository.DefaultTableName, PlansConstants.Database.SchemaName);
        });

        // Create and return the DbContext
        return new PlansDatabaseContext(optionsBuilder.Options);
    }
}
