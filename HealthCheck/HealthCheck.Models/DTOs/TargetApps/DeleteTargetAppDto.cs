﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class DeleteTargetAppDto :BaseRequestDto
    {
        public int Id { get; set; }

    }
}
