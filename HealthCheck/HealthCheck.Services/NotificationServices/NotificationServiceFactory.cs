using HealthCheck.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Services.NotificationServices
{
    public class NotificationServiceFactory
    {
        public static INotificationService GetNotificationService(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Email:
                    return new EmailNotificationService();
                case NotificationType.Sms:
                    return new EmailNotificationService();
                default:
                 return new EmailNotificationService();
            }


        }
    }
}
