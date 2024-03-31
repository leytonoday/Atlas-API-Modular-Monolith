using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Module;
using Quartz;

namespace Atlas.Shared.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessInboxJob<TCompositionRoot> : IJob
    where TCompositionRoot : ICompositionRoot
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor<TCompositionRoot>.SendCommand(new ProcessInboxCommand());
    }
}
