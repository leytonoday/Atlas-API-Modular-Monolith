using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Module;
using Quartz;

namespace Atlas.Shared.Infrastructure.BackgroundJobs;

/// <summary>
/// Implements a background job responsible for processing messages from the queue.
/// </summary>
/// <typeparam name="TCompositionRoot">The type of the composition root used for dependency resolution.</typeparam>
[DisallowConcurrentExecution]
public class ProcessQueueJob<TCompositionRoot>() : IJob
    where TCompositionRoot : ICompositionRoot
{
    private static readonly CommandsExecutor<TCompositionRoot> _commandsExecutor = new();

    /// <summary>
    /// Asynchronously executes the job, which in this case sends a <see cref="ProcessQueueCommand"/> 
    /// through the configured <see cref="CommandsExecutor{TCompositionRoot}"/>.
    /// </summary>
    /// <param name="context">The job execution context provided by the job runner.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Execute(IJobExecutionContext context)
    {
        await _commandsExecutor.SendCommand(new ProcessQueueCommand());
    }
}
