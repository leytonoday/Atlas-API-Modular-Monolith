using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Atlas.Shared;
using Atlas.Shared.Infrastructure.Options;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Atlas.Users.Infrastructure.Persistance;

public class UsersDatabaseContextFactory : IDesignTimeDbContextFactory<UsersDatabaseContext>
{
    public UsersDatabaseContext CreateDbContext(string[] args)
    {
        string environment = Utils.IsDevelopment() ? "Development" : "Production";

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{environment}.json")
            .Build();

        // Create DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<UsersDatabaseContext>();

        // Configure DbContext options
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Atlas"), options =>
        {
            options.MigrationsAssembly(typeof(UsersDatabaseContext).Assembly.GetName().Name);
            options.MigrationsHistoryTable(SqlServerHistoryRepository.DefaultTableName, UsersConstants.Database.SchemaName);
        });

        // Create and return the DbContext
        return new UsersDatabaseContext(optionsBuilder.Options);
    }
}
