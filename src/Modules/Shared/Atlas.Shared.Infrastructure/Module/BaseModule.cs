using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Autofac;

namespace Atlas.Shared.Infrastructure.Module;

public abstract class BaseModule<TCompositionRoot> : IModule 
    where TCompositionRoot : ICompositionRoot
{
    private readonly CommandsExecutor<TCompositionRoot> commandsExecutor = new();

    /// <summary>
    /// Sends a command to the module, and doesn't return any data.
    /// </summary>
    /// <param name="command">The command to be send to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when the operation is complete.</returns>
    public virtual async Task PublishNotification(INotification notification, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        await dispatcher.Publish(notification, cancellationToken);
    }

    /// <summary>
    /// Sends a command to the module and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the command.</typeparam>
    /// <param name="command">The command to send to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    public virtual async Task SendCommand(ICommand command, CancellationToken cancellationToken = default)
    {
        await commandsExecutor.SendCommand(command, cancellationToken);
    }

    /// <summary>
    /// Sends a query to the module and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the query.</typeparam>
    /// <param name="query">The query to be sent to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>Data of type <typeparamref name="TResult"/> that was returned from the query.</returns>
    public virtual async Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return await commandsExecutor.SendCommand(command, cancellationToken);
    }

    /// <summary>
    /// Publishes a notification to the module 
    /// </summary>
    /// <param name="notification">The notification to be published to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when the operation is complete.</returns>
    public virtual async Task<TResult> SendQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.Resolve<IMediator>();
        return await dispatcher.Send(query, cancellationToken);
    }
}
