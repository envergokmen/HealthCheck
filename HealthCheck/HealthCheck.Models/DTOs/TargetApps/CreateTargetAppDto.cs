﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class CreateTargetAppDto : BaseRequestDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Url { get; set; }
    }
}
