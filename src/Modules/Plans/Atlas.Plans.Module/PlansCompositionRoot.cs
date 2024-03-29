using Atlas.Shared.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Plans.Module;

/// <summary>
/// Responsible for configuring the Plan module's object graph. I.e., it's just the DI container for the module.
/// </summary>
public class PlansCompositionRoot : ICompositionRoot
{
    private static IServiceProvider? _provider;

    /// <inheritdoc/>
    public static IServiceScope BeginLifetimeScope() => _provider?.CreateScope() ?? throw new ServiceProviderNotSetException();

    /// <inheritdoc/>
    public static void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }
}
