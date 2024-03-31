using Quartz;

namespace Atlas.Shared.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        throw new NotImplementedException();
        //await CommandExecutor.SendCommand(new ProcessOutboxCommand());
    }
}
