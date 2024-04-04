﻿using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Shared.Infrastructure.Module;

public class CommandsExecutor<TCompositionRoot> where TCompositionRoot : ICompositionRoot
{
    public static async Task SendCommand(ICommand command, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        await dispatcher.Send(command, cancellationToken);
    }

    public static async Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        return await dispatcher.Send(command, cancellationToken);
    }
}
