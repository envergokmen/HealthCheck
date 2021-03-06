﻿using System.ComponentModel;

namespace HealthCheck.Models
{
    public enum Status
    {
        [Description("Active")]
        Active = 1,

        [Description("Passive")]
        Passive = 0,

        [Description("Deleted")]
        Deleted = 2
    }

    public enum Policy
    {
        Manager
    }
    public enum NotificationType
    {
        Email=0,
        Sms=1
    }

    public enum IntervalType
    {
        Minutely = 1,
        Hourly = 2,
        Daily = 3,
        Monthly = 5,
        Yearly = 6

    }
}
