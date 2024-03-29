using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Shared.Module;

public abstract class BaseModule<TCompositionRoot> : IModule where TCompositionRoot : ICompositionRoot
{
    /// <summary>
    /// Sends a command to the module, and doesn't return any data.
    /// </summary>
    /// <param name="command">The command to be send to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when the operation is complete.</returns>
    public async Task PublishNotification(INotification notification, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification, cancellationToken);
    }

    /// <summary>
    /// Sends a command to the module and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the command.</typeparam>
    /// <param name="command">The command to send to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>Data of type <typeparamref name="TResult"/> that was returned from the command.</returns>
    public async Task SendCommand(IRequest command, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command, cancellationToken);
    }
    /// <summary>
    /// Sends a query to the module and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the query.</typeparam>
    /// <param name="query">The query to be sent to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>Data of type <typeparamref name="TResult"/> that was returned from the query.</returns>
    public async Task<TResult> SendCommand<TResult>(IRequest<TResult> command, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(command, cancellationToken);
    }
    /// <summary>
    /// Publishes a notification to the module 
    /// </summary>
    /// <param name="notification">The notification to be published to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when the operation is complete.</returns>
    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();
        IMediator dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query, cancellationToken);
    }
}
