using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services;
using HealthCheck.Services.NotificationServices;

namespace HealthCheck.Web
{

    public class BgHealthCheckingService : IBackgroundHealthCheckerService
    {
        public readonly ITargetAppService _healthCheckService;
        public readonly IUserService _userService;

        public BgHealthCheckingService(IUserService userService, ITargetAppService healthCheckService)
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
                if (user == null || String.IsNullOrWhiteSpace(user.Email)) return;

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
