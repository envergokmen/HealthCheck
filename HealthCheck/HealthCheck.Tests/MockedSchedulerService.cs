using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services;
using System;
using System.Linq.Expressions;

namespace HealthCheck.Tests
{
    public class MockedSchedulerService : IJobScheduler
    {
        public void AddOrUpdate(TargetAppDto app)
        {
        }

        public void AddOrUpdate(TargetAppDto app, Expression<Action> methodCall)
        {
        }

        public void AddOrUpdate(int appId, Expression<Action> methodCall, IntervalType intervalType, int intervalValue)
        {
        }

        public void RemoveIfExists(int appId)
        {
        }
    }
}
