using Hangfire;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services.Utilities;
using HealthCheck.Web;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
            RecurringJob.AddOrUpdate("site-healthcheck-" + appId, methodCall, CronUtils.GetCronExpression(intervalType, intervalValue));
        }


        public void AddOrUpdate(TargetAppDto app)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + app.Id, ()=> _bgCheckService.CheckDownOrAlive(app), CronUtils.GetCronExpression(app.IntervalType, app.IntervalValue));
        }

        public void AddOrUpdate(TargetAppDto app, Expression<Action> methodCall)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + app.Id, methodCall, CronUtils.GetCronExpression(app.IntervalType, app.IntervalValue));
        }

        public void AddOrUpdate(UpdateTargetAppDto app, Expression<Action> methodCall)
        {
            RecurringJob.AddOrUpdate("site-healthcheck-" + app.Id, methodCall, CronUtils.GetCronExpression(app.IntervalType, app.IntervalValue));
        }

        public void RemoveIfExists(int appId)
        {
            RecurringJob.RemoveIfExists("site-healthcheck-" + appId);
        }
    }
}
