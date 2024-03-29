using Atlas.Shared.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Users.Module;

/// <summary>
/// Responsible for configuring the Users module's object graph. I.e., it's just the DI container for the module.
/// </summary>
public class UsersCompositionRoot : ICompositionRoot
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
