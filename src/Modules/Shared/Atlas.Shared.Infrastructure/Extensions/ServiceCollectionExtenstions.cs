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

namespace Atlas.Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers dependencies that are common between all modules.
    /// </summary>
    /// <typeparam name="TDatabaseContext">The type of DbContext used for database access.</typeparam>
    /// <typeparam name="TCompositionRoot">The type implementing the ICompositionRoot dependency resolution.</typeparam>
    /// <param name="services">The IServiceCollection instance to add services to.</param>
    /// <param name="configuration">An instance of IConfiguration for accessing application settings.</param>
    /// <returns>The updated IServiceCollection</returns>
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
        services.AddEmailServices();
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

    /// <summary>
    /// Configures various application options using dedicated setup classes.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to configure options.</param>
    /// <returns>The updated IServiceCollection</returns>
    public static IServiceCollection AddOptions(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<EmailOptionsSetup>()
            .ConfigureOptions<DatabaseOptionsSetup>()
            .ConfigureOptions<SupportNotificationOptionsSetup>();
    }

    /// <summary>
    /// Registers email-related services.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add services to.</param>
    /// <returns>The updated IServiceCollection</returns>
    public static IServiceCollection AddEmailServices(this IServiceCollection services)
    {
        services.AddMvcCore().AddRazorViewEngine(); // Enables MVC for potentially rendering email templates
        services.AddScoped<IEmailService, EmailService>(); // Registers the concrete implementation for sending emails
        services.AddScoped<EmailContentBuilder>(); // Registers a service for building email content
        return services;
    }

    /// <summary>
    /// Registers a singleton background task queue with a fixed concurrency limit.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add services to.</param>
    /// <returns>The updated IServiceCollection</returns>
    public static IServiceCollection AddBackgroundTaskQueue(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(_ =>
        {
            // Configure concurrency limit (100 in this case)
            return new BackgroundTaskQueue(100);
        });
        return services;
    }

    /// <summary>
    /// Registers database interceptors.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add services to.</param>
    /// <returns>The updated IServiceCollection</returns>
    public static IServiceCollection AddDatabaseInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>(); // Intercepts to update auditable entity properties
        services.AddSingleton<DomainEventPublisherInterceptor>(); // Intercepts to publish domain events after saving changes
        return services;
    }

    /// <summary>
    /// Registers validators from the provided assembly using FluentValidation.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add services to.</param>
    /// <param name="assembly">The assembly containing the validator classes.</param>
    /// <returns>The updated IServiceCollection</returns>
    public static IServiceCollection AddValidation(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly); // Registers validators using FluentValidation
        return services;
    }

}