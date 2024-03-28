using Atlas.Users.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Plans.Presentation;

public static class PlansPresentationDependencyInjection
{
    public static IServiceCollection AddPlansPresentationDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
