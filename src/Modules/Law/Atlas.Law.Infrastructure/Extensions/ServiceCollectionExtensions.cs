using Atlas.Law.Infrastructure.Persistance;
using Atlas.Shared.Application;
using Atlas.Shared.Domain;
using Atlas.Shared.Infrastructure.Behaviors;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Atlas.Law.Infrastructure.Module;
using FluentValidation;
using Atlas.Shared.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Infrastructure.Persistance.Interceptors;
using Atlas.Law.Application;

namespace Atlas.Law.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(LawApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(LawInfrastructureAssemblyReference).Assembly;
        var sharedApplicationAssembly = typeof(SharedApplicationAssemblyReference).Assembly;
        var sharedInfrastructureAssembly = typeof(SharedInfrastructureAssemblyReference).Assembly;

        // Commands Executor
        services.AddSingleton<ICommandsExecutor, CommandsExecutor<LawCompositionRoot>>();

        // Database related services
        services.AddLawDatabaseServices(configuration);

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

        return services;
    }

    public static IServiceCollection AddLawDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, LawUnitOfWork>();

        services.AddDbContext<LawDatabaseContext>((provider, options) =>
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

            var lawDatabaseContext = new LawDatabaseContext(options.Options as DbContextOptions<LawDatabaseContext>);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = lawDatabaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
                lawDatabaseContext.Database.Migrate();

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
            options.AddInterceptors(provider.GetRequiredService<DomainEventPublisherInterceptor>());
        });

        return services;
    }
}
