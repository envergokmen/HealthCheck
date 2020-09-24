using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        Gsm=1
    }

    public enum IntervalType
    {
        Seconds = 0,
        Minutes = 1,
        Hours = 2,
        Days = 3,
        Months = 4,
        Years = 4
    }
}
