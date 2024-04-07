using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Infrastructure.Module;

public interface ICommandsExecutor
{
    public Task SendCommand(ICommand command, CancellationToken cancellationToken = default);

    public Task<TResult> SendCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
