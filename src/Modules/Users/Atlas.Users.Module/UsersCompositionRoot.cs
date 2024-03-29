using Atlas.Shared.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Users.Module;

public class UsersCompositionRoot : ICompositionRoot
{
    private static IServiceProvider? _provider;

    public static IServiceScope BeginLifetimeScope() => _provider?.CreateScope() ?? throw new ServiceProviderNotSetException();

    public static void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }
}
