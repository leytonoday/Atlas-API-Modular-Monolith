using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Shared.Infrastructure.Module;

/// <summary>
/// Represents the interface for a Composition Root, responsible for configuring the application's object graph.
/// </summary>
public interface ICompositionRoot
{
    /// <summary>
    /// Sets the service provider to be used for dependency resolution within the Composition Root.
    /// </summary>
    /// <param name="provider">The service provider to be used for dependency resolution.</param>
    public abstract static void SetProvider(IServiceProvider provider);

    /// <summary>
    /// Begins a new scope for managing the lifetime of services within the Composition Root.
    /// </summary>
    /// <returns>An <see cref="IServiceScope"/> representing the new scope.</returns>
    public abstract static IServiceScope BeginLifetimeScope();
}
