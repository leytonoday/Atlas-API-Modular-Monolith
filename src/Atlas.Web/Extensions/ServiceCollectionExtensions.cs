using Atlas.Infrastructure.BackgroundJobs;
using Atlas.Plans.Application;
using Atlas.Plans.Infrastructure;
using Atlas.Plans.Presentation;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Application.Behaviors;
using Atlas.Shared.Infrastructure;
using Atlas.Shared.Infrastructure.BackgroundJobs;
using Atlas.Shared.Infrastructure.Persistance.Idempotance;
using Atlas.Shared.Presentation;
using Atlas.Users.Application;
using Atlas.Users.Infrastructure;
using Atlas.Users.Presentation;
using Atlas.Web.OptonsSetup;
using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Quartz;
using System.Reflection;
using Atlas.Plans.Infrastructure.Persistance;
using Atlas.Plans.Infrastructure.Persistance.Entities;
using Atlas.Users.Infrastructure.Persistance.Entities;
using Atlas.Users.Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Atlas.Users.Application.CQRS.Users.Events;
using System.Data;

namespace Atlas.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy(Constants.CorsPolicyName, builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            );
        });
    }

    public static void RegisterTrigger(
        this IServiceCollectionQuartzConfigurator quartzConfigurator,
        JobKey jobKey,
        int intervalInSeconds)
    {
        quartzConfigurator.AddTrigger(trigger => trigger
            .ForJob(jobKey)
            .WithSimpleSchedule(schedule => schedule
                .WithIntervalInSeconds(intervalInSeconds)
                .RepeatForever()));
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            // Process Background Task Queue Job Registration
            var processBackgroundTaskQueueJobKey = new JobKey(nameof(ProcessBackgroundTaskQueueJob));
            configure
                .AddJob<ProcessBackgroundTaskQueueJob>(processBackgroundTaskQueueJobKey)
                .RegisterTrigger(processBackgroundTaskQueueJobKey, 5); // Add the job with a schedule to run every X seconds, and repeat forever

            // Process Plans Outbox Messages Job Registration
            var processPlansOutboxMessagesJobKey = new JobKey(nameof(ProcessOutboxMessagesJob<PlansDatabaseContext, PlansOutboxMessage>) + "_Plans");
            configure
                .AddJob<ProcessOutboxMessagesJob<PlansDatabaseContext, PlansOutboxMessage>>(processPlansOutboxMessagesJobKey)
                .RegisterTrigger(processPlansOutboxMessagesJobKey, 5); // Add the job with a schedule to run every X seconds, and repeat forever

            // Process Users Outbox Messages Job Registration
            var processUsersOutboxMessagesJobKey = new JobKey(nameof(ProcessOutboxMessagesJob<UsersDatabaseContext, UsersOutboxMessage>) + "_Users");
            configure
                .AddJob<ProcessOutboxMessagesJob<UsersDatabaseContext, UsersOutboxMessage>>(processUsersOutboxMessagesJobKey)
                .RegisterTrigger(processUsersOutboxMessagesJobKey, 5); // Add the job with a schedule to run every X seconds, and repeat forever
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        var plansApplicationAssembly = typeof(PlansApplicationAssemblyReference).Assembly;
        var usersApplicationAssembly = typeof(UsersApplicationAssemblyReference).Assembly;

        services.AddValidatorsFromAssembly(plansApplicationAssembly);
        services.AddValidatorsFromAssembly(usersApplicationAssembly);

        return services;
    }

    public static IServiceCollection AddMediatrAndDecorators(this IServiceCollection services)
    {
        var plansApplicationAssembly = typeof(PlansApplicationAssemblyReference).Assembly;
        var usersApplicationAssembly = typeof(UsersApplicationAssemblyReference).Assembly;

        Assembly[] assemblies = [plansApplicationAssembly, usersApplicationAssembly];

        services.AddMediatR(config => config
            .RegisterServicesFromAssemblies(assemblies)
            .AddOpenBehavior(typeof(ValidationPipelineBehavior<,>)) // Register validation pipleline
            .NotificationPublisher = new ForeachAwaitPublisher()
        );

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<EmailOptionsSetup>()
            .ConfigureOptions<DatabaseOptionsSetup>()
            .ConfigureOptions<SupportNotificationOptionsSetup>()
            .ConfigureOptions<StripeOptionsSetup>();
    }

    public static IServiceCollection AddAutoMappings(this IServiceCollection services)
    {
        var plansInfrastructureAssembly = typeof(PlansInfrastructureAssemblyReference).Assembly;
        var usersInfrastructureAssembly = typeof(UsersInfrastructureAssemblyReference).Assembly;

        services.AddAutoMapper(plansInfrastructureAssembly);
        services.AddAutoMapper(usersInfrastructureAssembly);

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPlansInfrastructureDependencyInjection(configuration)
            .AddUsersInfrastructureDependencyInjection(configuration)
            .AddSharedInfrastructureDependencyInjection(configuration);
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        })
            .AddApplicationPart(typeof(UsersPresentationAssemblyReference).Assembly)
            .AddApplicationPart(typeof(PlansPresentationAssemblyReference).Assembly);

        return services
            .AddSharedPresentationDependencyInjection()
            .AddPlansPresentationDependencyInjection();
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddInfrastructure(configuration)
            .AddPresentation()
            .AddBackgroundJobs()
            .AddMediatrAndDecorators()
            .AddValidation()
            .AddOptions()
            .ConfigureCors()
            .AddAutoMappings();
    }
}
