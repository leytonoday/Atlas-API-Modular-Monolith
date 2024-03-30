﻿using Atlas.Plans.Domain;
using Atlas.Plans.Infrastructure.Persistance;
using Atlas.Shared.Infrastructure.Persistance.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Plans.Domain.Services;
using Atlas.Plans.Infrastructure.Services;
using Atlas.Plans.Infrastructure.Persistance.Entities;
using Atlas.Plans.Application;
using FluentValidation;

namespace Atlas.Plans.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PlanService>();
        services.AddScoped<IStripeService, StripeService>();

        // DB Related Services
        services.AddDatabaseServices(configuration);

        return services;
    }

    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPlansUnitOfWork, PlansUnitOfWork>();
        services.AddDbContextFactory<PlansDatabaseContext>((provider, options) =>
        {
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

            options.UseSqlServer(configuration.GetConnectionString("Atlas"), optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().Name);
                optionsBuilder.CommandTimeout(databaseOptions.CommandTimeout);
            });

            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);

            var plansDatabaseContext = new PlansDatabaseContext(options.Options as DbContextOptions<PlansDatabaseContext>);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = plansDatabaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
                plansDatabaseContext.Database.Migrate();

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
        });
        return services;
    }
}
