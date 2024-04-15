using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;

namespace Atlas.Shared.Infrastructure.Module;

/// <summary>
/// Represends a module in a modular monolith architecture. Acts as a facade between the API and the module.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Sends a command to the module, and doesn't return any data.
    /// </summary>
    /// <param name="command">The command to be send to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when the operation is complete.</returns>
    public abstract Task SendCommand(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command to the module and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the command.</typeparam>
    /// <param name="command">The command to send to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>Data of type <typeparamref name="TResult"/> that was returned from the command.</returns>
    public abstract Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a query to the module and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the query.</typeparam>
    /// <param name="query">The query to be sent to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>Data of type <typeparamref name="TResult"/> that was returned from the query.</returns>
    public abstract Task<TResult> SendQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to the module 
    /// </summary>
    /// <param name="notification">The notification to be published to the module.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when the operation is complete.</returns>
    public abstract Task PublishNotification(INotification notification, CancellationToken cancellationToken = default);
}
