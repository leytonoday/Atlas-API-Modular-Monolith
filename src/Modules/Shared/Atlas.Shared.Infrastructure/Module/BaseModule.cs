using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Autofac;

namespace Atlas.Shared.Infrastructure.Module;

/// <inheritdoc/>
public abstract class BaseModule<TCompositionRoot> : IModule 
    where TCompositionRoot : ICompositionRoot
{
    private readonly CommandsExecutor<TCompositionRoot> commandsExecutor = new();

    /// <inheritdoc/>
    public virtual async Task PublishNotification(INotification notification, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        await dispatcher.Publish(notification, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task SendCommand(ICommand command, CancellationToken cancellationToken = default)
    {
        await commandsExecutor.SendCommand(command, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return await commandsExecutor.SendCommand(command, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult> SendQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        return await dispatcher.Send(query, cancellationToken);
    }
}
