using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.Behaviors;
using Atlas.Shared.Infrastructure.Builders;
using Atlas.Shared.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Atlas.Shared.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        //// domain events
        //services.AddScoped<DomainEventAccessor>();
        //services.AddScoped<DomainEventPublisher>();
        //services.AddScoped<OutboxMessagePublisher>();

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddScoped<ISupportNotifierService, SupportNotifierService>()
            .AddEmailServices()
            .AddBackgroundTaskQueue()
            .AddDatabaseInterceptors();

        return services;
    }

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
        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }

    public static IServiceCollection AddAutoMappings(this IServiceCollection services, Assembly assembly)
    {
        services.AddAutoMapper(assembly);
        return services;
    }
}