using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Behaviors;
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
        //SqlMapper.AddTypeHandler(OrganisationIdTypeHandler.Default);
        //SqlMapper.AddTypeHandler(UserIdTypeHandler.Default);
        //SqlMapper.AddTypeHandler(ProjectIdTypeHandler.Default);
        //SqlMapper.AddTypeHandler(EmailAddressTypeHandler.Default);
        //SqlMapper.AddTypeHandler(PasswordTypeHandler.Default);

        //// domain events
        //services.AddScoped<DomainEventAccessor>();
        //services.AddScoped<DomainEventPublisher>();
        //services.AddScoped<OutboxMessagePublisher>();

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }

    public static IServiceCollection AddExecutionContextAccessor(this IServiceCollection services)
    {
        return services.AddSingleton<IExecutionContextAccessor, IExecutionContextAccessor>();
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