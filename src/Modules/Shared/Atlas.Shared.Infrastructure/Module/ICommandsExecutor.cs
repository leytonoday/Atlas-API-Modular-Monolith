using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Infrastructure.Module;

/// <summary>
/// Represents an executor for commands, responsible for sending commands to the appropriate handlers within a module.
/// </summary>
public interface ICommandsExecutor
{
    /// <summary>
    /// Sends a command to be executed asynchronously.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendCommand(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command to be executed asynchronously and expects a result.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the command.</typeparam>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the result of the command.</returns>
    Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
