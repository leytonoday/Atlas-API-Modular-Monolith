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
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Infrastructure.Persistance.Repositories;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Shared.Domain;
using Atlas.Shared.Application;
using Atlas.Shared.Infrastructure.Options;
using Atlas.Plans.Infrastructure.Options.OptionSetup;
using Atlas.Shared.Infrastructure;
using Atlas.Shared.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Plans.Infrastructure.Module;
using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Atlas.Plans.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        var applicationAssembly = typeof(PlansApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(PlansInfrastructureAssemblyReference).Assembly;
        var sharedApplicationAssembly = typeof(SharedApplicationAssemblyReference).Assembly;
        var sharedInfrastructureAssembly = typeof(SharedInfrastructureAssemblyReference).Assembly;

        // Commands Executor
        services.AddSingleton<ICommandsExecutor, CommandsExecutor<PlansCompositionRoot>>();

        // Database related services
        services.AddPlansDatabaseServices(configuration);

        // MediatR
        var assemblies = new[] { infrastructureAssembly, applicationAssembly, sharedApplicationAssembly, sharedInfrastructureAssembly };
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
        return services.ConfigureOptions<StripeOptionsSetup>();
    }

    public static IServiceCollection AddPlansDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, PlansUnitOfWork>();

        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IFeatureRepository, FeatureRepository>();
        services.AddScoped<IStripeCustomerRepository, StripeCustomerRepository>();
        services.AddScoped<ICreditTrackerRepository, CreditTrackerRepository>();
        services.AddScoped<IStripeCardFingerprintRepository, StripeCardFingerprintRepository>();

        services.AddDbContext<PlansDatabaseContext>((provider, options) =>
        {
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

            options.UseSqlServer(configuration.GetConnectionString("Atlas"), optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().Name);
                optionsBuilder.CommandTimeout(databaseOptions.CommandTimeout);
                optionsBuilder.MigrationsHistoryTable(SqlServerHistoryRepository.DefaultTableName, PlansConstants.Database.SchemaName);
            });

            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);

            var plansDatabaseContext = new PlansDatabaseContext(options.Options as DbContextOptions<PlansDatabaseContext>);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = plansDatabaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
            {
                try
                {
                    plansDatabaseContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
            options.AddInterceptors(provider.GetRequiredService<DomainEventPublisherInterceptor>());
        });

        return services;
    }
}
