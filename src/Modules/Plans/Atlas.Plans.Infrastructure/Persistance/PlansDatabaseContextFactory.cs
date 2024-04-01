using Atlas.Shared.Infrastructure.Persistance.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Atlas.Shared;

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
        });

        // Create and return the DbContext
        return new PlansDatabaseContext(optionsBuilder.Options);
    }
}
