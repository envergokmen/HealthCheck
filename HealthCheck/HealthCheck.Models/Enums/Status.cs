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
}
