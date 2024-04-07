using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Module;
using Quartz;

namespace Atlas.Shared.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxJob<TCompositionRoot> : IJob
    where TCompositionRoot : ICompositionRoot
{
    private static readonly CommandsExecutor<TCompositionRoot> _commandsExecutor = new();

    public async Task Execute(IJobExecutionContext context)
    {
        await _commandsExecutor.SendCommand(new ProcessOutboxCommand());
    }
}
