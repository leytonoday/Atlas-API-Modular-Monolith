using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Infrastructure.Persistance;
using Atlas.Shared.Infrastructure.Persistance.Options;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Users.Infrastructure;

public static class UsersInfrastructureDependencyInjection
{
    /// <summary>
    /// Adds Identity services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 10;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.AllowedUserNameCharacters = UsersConstants.Identity.AllowedUserNameCharacters;
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

    public static IServiceCollection AddUsersInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<UsersDatabaseContext>((provider, options) =>
        {
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

            options.UseSqlServer(configuration.GetConnectionString("Atlas"), optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(UsersInfrastructureDependencyInjection).Assembly.GetName().Name);
                optionsBuilder.CommandTimeout(databaseOptions.CommandTimeout);
            });

            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);

            var applicationDatabaseContext = new UsersDatabaseContext(options.Options);

            // Apply any migrations that have yet to be applied
            IEnumerable<string> migrationsToApply = applicationDatabaseContext.Database.GetPendingMigrations();
            if (migrationsToApply.Any())
                applicationDatabaseContext.Database.Migrate();

            // Register database interceptors
            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>());
            options.AddInterceptors(provider.GetRequiredService<DomainEventToOutboxMessageInterceptor>());
        });

        services.AddIdentity();

        return services;
    }
}
