using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services
{
    public interface INotificationService
    {
        bool NotifyAppDown(TargetAppDto app);
    }

    public class NotificationService
    {
        private readonly HealthContext db;

        public NotificationService(HealthContext _db)
        {
            db = _db;
        }
 
    }
}
