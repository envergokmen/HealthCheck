using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services
{

    public class EmailNotificationService : INotificationService
    {
        public void NotifyDown(UserDto user, TargetAppDto targetApp)
        {
            //send email
            Console.WriteLine($"Sending email to:{user.Name} - {user.Email} for down info of : {targetApp.Id} - {targetApp.Url}");

        }

    }
}
