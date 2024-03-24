using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Builders;
using Atlas.Shared.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Atlas.Infrastructure.Persistance.Interceptors;

namespace Atlas.Shared.Infrastructure;

public static class SharedInfrastructureDependencyInjection
{
    public static IServiceCollection AddEmailServices(this IServiceCollection services)
    {
        services.AddMvcCore().AddRazorViewEngine();
        services.AddScoped<RazorViewToStringRenderer>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<EmailContentBuilder>();

        return services;
    }

    public static IServiceCollection AddBackgroundTaskQueue(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(_ =>
            new BackgroundTaskQueue(100)
        );

        return services;
    }

    public static IServiceCollection AddDatabaseInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddSingleton<DomainEventToOutboxMessageInterceptor>();

        return services;
    }

    public static IServiceCollection AddSharedInfrastructureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddScoped<ISupportNotifierService, SupportNotifierService>()
            .AddEmailServices()
            .AddBackgroundTaskQueue()
            .AddDatabaseInterceptors();

        return services;
    }
}
