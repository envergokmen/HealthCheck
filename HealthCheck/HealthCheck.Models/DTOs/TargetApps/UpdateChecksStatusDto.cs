﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class UpdateChecksStatusDto : BaseRequestDto
    {
        public int Id { get; set; }
        public bool IsAlive { get; set; }
        public DateTime CheckDate { get; set; }

    }
}
