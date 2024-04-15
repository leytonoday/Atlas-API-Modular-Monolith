namespace Atlas.Shared.Infrastructure.Decorators;

/// <summary>
/// Internal marker interface used to identify request or notification handler decorators.
/// </summary>
internal interface IIsDecorator
{
    /// <summary>
    /// Static method to check if a given type implements the <see cref="IIsDecorator"/> interface.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <returns>True if the type implements <see cref="IIsDecorator"/>, false otherwise.</returns>
    public static bool IsDecorator(Type type)
    {
        return typeof(IIsDecorator).IsAssignableFrom(type);
    }
}
