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
using Atlas.Law.Infrastructure.Options;
using Atlas.Law.Application.Services;
using Atlas.Law.Infrastructure.Services.PineconeService;
using Atlas.Law.Infrastructure.Services.LargeLanguageModelService;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Infrastructure.Persistance.Repositories;
using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Law.Infrastructure.Services.LanguageDetector;

namespace Atlas.Law.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(LawApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(LawInfrastructureAssemblyReference).Assembly;
        var sharedApplicationAssembly = typeof(SharedApplicationAssemblyReference).Assembly;
        var sharedInfrastructureAssembly = typeof(SharedInfrastructureAssemblyReference).Assembly;

        services.AddOptions();

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

        // Services
        services.AddScoped<IVectorDatabaseService, PineconeService>();
        services.AddScoped<ILargeLanguageModelService, OpenAiLargeLanguageModelService>();
        services.AddScoped<ILanguageDetector, GoogleTranslateLanguageDetector>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services) 
    {
        services.ConfigureOptions<PineconeOptionsSetup>();
        services.ConfigureOptions<OpenAiOptionsSetup>();
        services.ConfigureOptions<GoogleTranslateOptionsSetup>();
        return services;
    }

    public static IServiceCollection AddLawDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, LawUnitOfWork>();

        services.AddScoped<ILegalDocumentRepository, LegalDocumentRepository>();
        services.AddScoped<IEurLexSumDocumentRepository, EurLexSumDocumentRepository>();

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
