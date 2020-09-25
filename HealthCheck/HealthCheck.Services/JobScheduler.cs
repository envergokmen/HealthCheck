using Hangfire;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Web;
using System;
using System.Linq.Expressions;

namespace HealthCheck.Services
{
    public class JobScheduler : IJobScheduler
    {
        private readonly IBackgroundHealthCheckerService _bgCheckService;

        public JobScheduler(IBackgroundHealthCheckerService bgCheckService)
        {
            _bgCheckService = bgCheckService;
        }


        public void AddOrUpdate(int appId, Expression<Action> methodCall, IntervalType intervalType, int intervalValue)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + appId, methodCall, GetCronExpression(intervalType, intervalValue));
        }


        public void AddOrUpdate(TargetAppDto app)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + app.Id, ()=> _bgCheckService.CheckDownOrAlive(app), GetCronExpression(app.IntervalType, app.IntervalValue));
        }

        public void AddOrUpdate(TargetAppDto app, Expression<Action> methodCall)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + app.Id, methodCall, GetCronExpression(app.IntervalType, app.IntervalValue));
        }

        public void AddOrUpdate(UpdateTargetAppDto app, Expression<Action> methodCall)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + app.Id, methodCall, GetCronExpression(app.IntervalType, app.IntervalValue));
        }

        public void RemoveIfExists(int appId)
        {
            RecurringJob.RemoveIfExists("site-healthcheck-" + appId);
        }

        public string GetCronExpression(IntervalType intervalType, int value)
        {
            switch (intervalType)
            {
                case IntervalType.Minutely:
                    return Cron.MinuteInterval(value);
                case IntervalType.Hourly:
                    return Cron.HourInterval(value);
                case IntervalType.Daily:
                    return Cron.DayInterval(value);
                case IntervalType.Monthly:
                    return Cron.MonthInterval(value);
            }

            throw new Exception("please select a valid interval type");
        }
    }
}
