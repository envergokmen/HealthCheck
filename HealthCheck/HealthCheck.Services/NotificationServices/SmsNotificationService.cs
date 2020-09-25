using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Models.DTOs.User;
using System;

namespace HealthCheck.Services.NotificationServices
{

    public class SmsNotificationService : INotificationService
    {
      
        public void NotifyDown(UserDto user, TargetAppDto targetApp)
        {
            Console.WriteLine($"Sending sms to:{user.Name} - {user.Gsm} for down info of : {targetApp.Id} - {targetApp.Url}");
            //send sms
        }

    }
}
