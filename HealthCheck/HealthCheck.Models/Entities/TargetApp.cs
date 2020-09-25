using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCheck.Models
{
    public class TargetApp : DbBaseObject
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Url { get; set; }
        public DateTime? LastCheck { get; set; }
        public bool? IsAlive { get; set; }
        public IntervalType IntervalType { get; set; }
        public int IntervalValue { get; set; }
    }
}
