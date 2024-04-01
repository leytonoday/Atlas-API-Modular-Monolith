﻿using Atlas.Plans.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Plans.Domain.Services;
using Atlas.Plans.Infrastructure.Services;
using Atlas.Plans.Application;
using FluentValidation;
using MediatR;
using Atlas.Shared.Infrastructure.Behaviors;
using Atlas.Users.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Infrastructure.Persistance.Repositories;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Shared.Domain;
using Atlas.Shared.Application;
using System.Reflection;
using Atlas.Shared.Infrastructure.Options;
using Atlas.Plans.Infrastructure.Options.OptionSetup;

namespace Atlas.Plans.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        var applicationAssembly = typeof(PlansApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(PlansInfrastructureAssemblyReference).Assembly;

        // Database related services
        services.AddDatabaseServices(configuration);

        // MediatR
        var assemblies = new[] { Assembly.GetExecutingAssembly(), typeof(SharedApplicationAssemblyReference).Assembly };
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        // Validation
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Auto-mapper
        services.AddAutoMapper(infrastructureAssembly);

        // Services
        services.AddScoped<PlanService>();
        services.AddScoped<IStripeService, StripeService>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<StripeOptionsSetup>();
    }

    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, PlansUnitOfWork>();

        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IFeatureRepository, FeatureRepository>();
        services.AddScoped<IPlanFeatureRepository, PlanFeatureRepository>();
        services.AddScoped<IStripeCustomerRepository, StripeCustomerRepository>();
        services.AddScoped<IStripeCardFingerprintRepository, StripeCardFingerprintRepository>();

        services.AddDbContext<PlansDatabaseContext>((provider, options) =>
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
