using Atlas.Shared.Infrastructure.Decorators;
using Autofac;
using MediatR;

namespace Atlas.Shared.Infrastructure.Extensions;

public static class ContainerBuilderExtensions
{
    /// <summary>
    /// For registering services using features that are only supported by Autofac, such as decorators.
    /// </summary>
    /// <param name="containerBuilder"></param>
    public static void AddAutofacServices(this ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterGenericDecorator(typeof(RequestIdempotenceDecorator<>), typeof(IRequestHandler<>));
        containerBuilder.RegisterGenericDecorator(typeof(NotificationIdempotenceDecorator<>), typeof(INotificationHandler<>));
    }
}