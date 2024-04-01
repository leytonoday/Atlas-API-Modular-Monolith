using Atlas.Plans.Application;
using Atlas.Plans.Infrastructure;
using Atlas.Plans.Presentation;
using Atlas.Shared.Application.Behaviors;
using Atlas.Shared.Infrastructure;
using Atlas.Shared.Infrastructure.BackgroundJobs;
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
using Atlas.Users.Infrastructure.Persistance;
using Atlas.Shared.Application.Abstractions;
using Atlas.Web.ExecutionContext;

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

    public static IServiceCollection AddOptions(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<EmailOptionsSetup>()
            .ConfigureOptions<DatabaseOptionsSetup>()
            .ConfigureOptions<SupportNotificationOptionsSetup>()
            .ConfigureOptions<StripeOptionsSetup>();
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
            .AddSharedPresentationDependencyInjection();
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {

        return services
            .AddPresentation()
            .ConfigureCors();
    }
}
