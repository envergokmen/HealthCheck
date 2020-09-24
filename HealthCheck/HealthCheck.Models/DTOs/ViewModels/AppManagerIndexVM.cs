using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Models.DTOs.ViewModels
{
    public class AppManagerIndexVM
    {
        public AppManagerIndexVM(UserDto user, List<TargetAppDto>  apps)
        {
            User = user;
            Apps = apps;
        }
        public AppManagerIndexVM(UserDto user)
        {
            User = user;
        }

        public UserDto User { get; set; }
        public List<TargetAppDto> Apps { get; set; }

    }
}
