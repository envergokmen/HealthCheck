using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class NotifyAppDownDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }

    }
}
