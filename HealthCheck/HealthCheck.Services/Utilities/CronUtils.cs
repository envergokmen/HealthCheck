using Hangfire;
using HealthCheck.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Services.Utilities
{
    public class CronUtils
    {
        public static string GetCronExpression(IntervalType intervalType, int value)
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
