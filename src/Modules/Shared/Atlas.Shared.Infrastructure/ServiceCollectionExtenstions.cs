using Atlas.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.Behaviors;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Builders;
using Atlas.Shared.Infrastructure.CommandQueue;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using Atlas.Shared.Infrastructure.Integration.Outbox;
using Atlas.Shared.Infrastructure.Queue;
using Atlas.Shared.Infrastructure.Services;
using FluentValidation;
using MediatR;
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

        // MediatR pipeline behaviours
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        // External communications
        services.AddScoped<ISupportNotifierService, SupportNotifierService>();
        services.AddEmailServices();

        // Background task queue
        services.AddBackgroundTaskQueue();

        // Database
        services.AddDatabaseInterceptors();

        // Command queue
        services.AddScoped<IQueueWriter, QueueWriter>();
        services.AddScoped<QueueReader>();

        // Inbox
        services.AddScoped<IInboxWriter, IInboxWriter>();
        services.AddScoped<InboxReader>();

        // Outbox
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<OutboxReader>();


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
        
        return services;
    }
}