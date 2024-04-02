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
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.Options.OptonsSetup;
using Atlas.Shared.Infrastructure.Persistance.Interceptors;
using Atlas.Shared.Infrastructure.Queue;
using Atlas.Shared.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Atlas.Shared.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommon<TDatabaseContext, TCompositionRoot>(this IServiceCollection services, IConfiguration configuration)
        where TDatabaseContext : DbContext
        where TCompositionRoot : ICompositionRoot
    {
        services.AddSingleton(configuration);

        services.AddOptions();

        // MediatR pipeline behaviours
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        // External communications
        services.AddEmailServices<TCompositionRoot>();
        services.AddScoped<ISupportNotifierService, SupportNotifierService>();

        // Background task queue
        services.AddBackgroundTaskQueue();

        // Database
        services.AddDatabaseInterceptors();

        // Command queue
        services.AddScoped<IQueueWriter, QueueWriter<TDatabaseContext>>();
        services.AddScoped<IQueueReader, QueueReader<TDatabaseContext>>();

        // Inbox
        services.AddScoped<IInboxWriter, InboxWriter<TDatabaseContext>>();
        services.AddScoped<IInboxReader, InboxReader<TDatabaseContext>>();

        // Outbox
        services.AddScoped<IOutboxWriter, OutboxWriter<TDatabaseContext>>();
        services.AddScoped<IOutboxReader, OutboxReader<TDatabaseContext>>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<EmailOptionsSetup>()
            .ConfigureOptions<DatabaseOptionsSetup>()
            .ConfigureOptions<SupportNotificationOptionsSetup>();
    }

    public static IServiceCollection AddEmailServices<TCompositionRoot>(this IServiceCollection services) where TCompositionRoot : ICompositionRoot
    {
        services.AddMvcCore().AddRazorViewEngine();
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
        services.AddSingleton<DomainEventPublisherInterceptor>();
        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}