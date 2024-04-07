using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Autofac;
using MediatR;

namespace Atlas.Shared.Infrastructure.Module;

public class CommandsExecutor<TCompositionRoot> : ICommandsExecutor where TCompositionRoot : ICompositionRoot
{
    public async Task SendCommand(ICommand command, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        await dispatcher.Send(command, cancellationToken);
    }

    public async Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        return await dispatcher.Send(command, cancellationToken);
    }
}
