using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Application;
using Atlas.Shared.Domain;
using Atlas.Shared.Infrastructure;
using Atlas.Shared.Infrastructure.Behaviors;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.Options;
using Atlas.Shared.Infrastructure.Persistance.Interceptors;
using Atlas.Users.Application;
using Atlas.Users.Domain;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Infrastructure.Persistance;
using Atlas.Users.Infrastructure.Persistance.Repositories;
using Atlas.Users.Module;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(UsersApplicationAssemblyReference).Assembly;
        var infrastructureAssembly = typeof(UsersInfrastructureAssemblyReference).Assembly;
        var sharedApplicationAssembly = typeof(SharedApplicationAssemblyReference).Assembly;
        var sharedInfrastructureAssembly = typeof(SharedInfrastructureAssemblyReference).Assembly;

        // Commands Executor
        services.AddSingleton<ICommandsExecutor, CommandsExecutor<UsersCompositionRoot>>();

        // Database related services
        services.AddUsersDatabaseServices(configuration);

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

        // Identity
        services.AddIdentity();

        return services;
    }

    public static IServiceCollection AddUsersDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UsersUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<UsersDatabaseContext>((provider, options) =>
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

            var usersDatabaseContext = new UsersDatabaseContext(options.Options as DbContextOptions<UsersDatabaseContext>);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = usersDatabaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
                usersDatabaseContext.Database.Migrate();

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
            options.AddInterceptors(provider.GetRequiredService<DomainEventPublisherInterceptor>());
        });

        return services;
    }

    /// <summary>
    /// Adds Identity services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
           .AddEntityFrameworkStores<UsersDatabaseContext>()
           .AddDefaultTokenProviders();

        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            // Cookies are validated every 3 minutes. If a user is signed out, their security stamp is updated,
            // and within at the most 3 minutes, they'll be unable to access this API using their account
            options.ValidationInterval = TimeSpan.FromMinutes(3);
        });

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            // All tokens that are generated using the UserManager will expire within 30 minutes. 
            options.TokenLifespan = TimeSpan.FromMinutes(30);
        });

        return services;
    }

}
