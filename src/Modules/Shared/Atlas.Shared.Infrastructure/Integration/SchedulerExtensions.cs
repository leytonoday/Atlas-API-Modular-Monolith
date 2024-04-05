﻿using Quartz;

namespace Atlas.Shared.Infrastructure.Integration;

public static class SchedulerExtensions
{
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
