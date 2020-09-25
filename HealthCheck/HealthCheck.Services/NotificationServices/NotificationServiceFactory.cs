using HealthCheck.Models;

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
