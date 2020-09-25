using System;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class TargetAppDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public IntervalType IntervalType { get; set; }
        public int IntervalValue { get; set; }
        public DateTime? LastCheck { get; set; }
        public bool? IsAlive { get; set; }

        public int? CreatedById { get; set; }

    }
}
