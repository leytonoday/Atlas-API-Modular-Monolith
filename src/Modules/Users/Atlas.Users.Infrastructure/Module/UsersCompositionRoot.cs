﻿using Atlas.Shared.Infrastructure.Module;
using Autofac;

namespace Atlas.Users.Infrastructure.Module;

/// <summary>
/// Responsible for configuring the Users module's object graph. I.e., it's just the DI container for the module.
/// </summary>
public class UsersCompositionRoot : ICompositionRoot
{
    private static IContainer? _container;

    /// <inheritdoc/>
    public static ILifetimeScope BeginLifetimeScope() => _container?.BeginLifetimeScope() ?? throw new ServiceProviderNotSetException();

    /// <inheritdoc/>
    public static void SetContainer(IContainer container)
    {
        _container = container;
    }
}
