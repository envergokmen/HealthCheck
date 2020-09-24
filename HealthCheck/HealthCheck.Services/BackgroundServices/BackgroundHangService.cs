using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services;

namespace HealthCheck.Web
{

    public class BackgroundHangService : IBackgroundHangService
    {
        public readonly HealthCheckService _healthCheckService;
        public readonly UserService _userService;

        public BackgroundHangService(UserService userService, HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
            _userService = userService;
        }

        public void CheckDownOrAlive(TargetAppDto item)
        {
            if (item == null) return;

            Debug.WriteLine($"Processing {item.Id}  -  {item.Name} url : {item.Url}");

            var isAlive = CheckIsAlive(item.Url);
            NotifyUser(item, isAlive);
            _healthCheckService.MarkAsChecked(new UpdateChecksStatusDto { CheckDate = DateTime.Now, IsAlive = isAlive, Id = item.Id });

        }

        private void NotifyUser(TargetAppDto item, bool isAlive)
        {
            if (!isAlive)
            {
                var user = _userService.GetById(item.CreatedById.GetValueOrDefault(0));
                var notificationService = NotificationServiceFactory.GetNotificationService(user.NotificationPreference);
                notificationService.NotifyDown(user, item);
            }
        }

        private static bool CheckIsAlive(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var response = httpClient.GetAsync(url).Result;
                var statusCodeNumber = (int)response.StatusCode;
                return statusCodeNumber >= 200 && statusCodeNumber < 300 ? true : false;
            }

        }

    }
}
