using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class UpdateTargetAppDto : BaseRequestDto
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

    }
}
