using Quartz;

namespace Atlas.Shared.Infrastructure.Integration;

/// <summary>
/// Extension methods for scheduling jobs with Quartz Scheduler.
/// </summary>
public static class SchedulerExtensions
{
    /// <summary>
    /// Adds a scheduled job to the Quartz Scheduler for processing.
    /// </summary>
    /// <typeparam name="TJob">The type of job to add, which must implement <see cref="IJob"/>.</typeparam>
    /// <param name="scheduler">The instance of the Quartz Scheduler.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task AddMessageboxJob<TJob>(this IScheduler scheduler) where TJob : IJob
    {
        var name = typeof(TJob).Name;
        var job = JobBuilder.Create<TJob>().WithIdentity(name).Build();
        var trigger = TriggerBuilder.Create()
            .StartNow()
            .WithIdentity(name)
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}
