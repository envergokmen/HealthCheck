using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HealthCheck.Services
{
    public interface IJobScheduler
    {
        void AddOrUpdate(TargetAppDto app);
        void AddOrUpdate(TargetAppDto app, Expression<Action> methodCall);
        void AddOrUpdate(int appId, Expression<Action> methodCall, IntervalType intervalType, int intervalValue);
        void RemoveIfExists(int appId);
    }
}
