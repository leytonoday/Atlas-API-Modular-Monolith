using Atlas.Infrastructure.BackgroundJobs;
using Atlas.Plans.Application;
using Atlas.Plans.Infrastructure;
using Atlas.Shared.Application.Behaviors;
using Atlas.Shared.Infrastructure.BackgroundJobs;
using Atlas.Shared.Infrastructure.Persistance.Idempotance;
using Atlas.Users.Application;
using Atlas.Users.Infrastructure;
using Atlas.Web.OptonsSetup;
using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Quartz;
using System.Reflection;

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

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            // Process Background Task Queue Job Registration
            var processBackgroundTaskQueueJobKey = new JobKey(nameof(ProcessBackgroundTaskQueueJob));
            // Add the job with a schedule to run every X seconds, and repeat forever
            configure
                .AddJob<ProcessBackgroundTaskQueueJob>(processBackgroundTaskQueueJobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(processBackgroundTaskQueueJobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));

            // Process Outbox Messages Job Registration
            var processOutboxMessagesJobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            // Add the job with a schedule to run every X seconds, and repeat forever
            configure
                .AddJob<ProcessOutboxMessagesJob>(processOutboxMessagesJobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(processOutboxMessagesJobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));
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

        // Before any notification handler is invoked, run the DomainEventHandlerIdempotenceDecorator beforehand as a proxy
        services.Decorate(typeof(INotificationHandler<>), typeof(DomainEventHandlerIdempotenceDecorator<,>));

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

    public static IServiceCollection AddConfigurations(this IServiceCollection services)
    {
        return services
            .AddBackgroundJobs()
            .AddMediatrAndDecorators()
            .AddValidation()
            .AddOptions()
            .ConfigureCors()
            .AddAutoMappings();
    }
}
