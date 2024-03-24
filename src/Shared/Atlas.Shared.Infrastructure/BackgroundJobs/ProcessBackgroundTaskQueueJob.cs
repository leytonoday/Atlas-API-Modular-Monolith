using Quartz;
using Microsoft.Extensions.Logging;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Shared.Infrastructure.BackgroundJobs;

/// <summary>
/// Represents a Quartz.NET job for processing background tasks from an <see cref="IBackgroundTaskQueue"/>.
/// </summary>
/// <param name="backgroundTaskQueue">The background task queue to process tasks from.</param>
/// <param name="logger">The logger for recording job execution information.</param>
[DisallowConcurrentExecution]
public class ProcessBackgroundTaskQueueJob(IBackgroundTaskQueue backgroundTaskQueue, ILogger<ProcessBackgroundTaskQueueJob> logger) : IJob
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue = backgroundTaskQueue;
    private readonly ILogger<ProcessBackgroundTaskQueueJob> _logger = logger;

    /// <summary>
    /// Executes the background job, processing tasks from the associated <see cref="IBackgroundTaskQueue"/>.
    /// </summary>
    /// <param name="context">The execution context provided by Quartz.NET.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        // Get all jobs that have been addedd to the queue since the last execution of this method as per the Quartz schedule
        uint jobCount = _backgroundTaskQueue.GetJobCount();
        for (int i = 0; i < jobCount; i++)
        {
            // Stop processing jobs if cancellation has been requested
            if (context.CancellationToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                // Dequeue item
                Func<CancellationToken, ValueTask>? task =
                    await _backgroundTaskQueue.DequeueAsync(context.CancellationToken);

                // Should never occur
                if (task is null) 
                    continue;

                // Process task
                await task(context.CancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if cancellationToken was signaled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
        }
    }
}
