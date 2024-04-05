using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Shared.Infrastructure.Decorators;

/// <summary>
/// Marker interface used to denote if some request or notification handler is actually a decorator.
/// </summary>
internal interface IIsDecorator
{
    public static bool IsDecorator(Type type)
    {
        Type iIsDecorator = typeof(IIsDecorator);

        return iIsDecorator.IsAssignableFrom(type);
    }
}