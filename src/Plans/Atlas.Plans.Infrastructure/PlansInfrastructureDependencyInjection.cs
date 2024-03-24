using Atlas.Plans.Domain;
using Atlas.Plans.Infrastructure.Persistance;
using Atlas.Shared.Infrastructure.Persistance.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Plans.Domain.Services;

namespace Atlas.Plans.Infrastructure;

public static class PlansInfrastructureDependencyInjection
{
    public static IServiceCollection AddPlansInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPlansUnitOfWork, PlansUnitOfWork>();

        services.AddDbContextFactory<PlansDatabaseContext>((provider, options) =>
        {
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

            options.UseSqlServer(configuration.GetConnectionString("Atlas"), optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(PlansInfrastructureDependencyInjection).Assembly.GetName().Name);
                optionsBuilder.CommandTimeout(databaseOptions.CommandTimeout);
            });

            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);

            var applicationDatabaseContext = new PlansDatabaseContext(options.Options);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = applicationDatabaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
                applicationDatabaseContext.Database.Migrate();

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
            options.AddInterceptors(provider.GetRequiredService<DomainEventToOutboxMessageInterceptor>());
        });


        services.AddScoped<PlanService>();

        return services;
    }
}
