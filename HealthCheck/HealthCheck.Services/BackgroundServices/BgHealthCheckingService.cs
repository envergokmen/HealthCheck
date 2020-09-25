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

        public BgHealthCheckingService(IUserService userService, ITargetAppService targetAppService)
        {
            _healthCheckService = targetAppService;
            _userService = userService;
        }

        public AppCheckResultDto CheckDownOrAlive(TargetAppDto item)
        {
            if (item == null) return null;
            AppCheckResultDto result = new AppCheckResultDto();
           
            //left on purpos in order to track
            Debug.WriteLine($"Processing {item.Id}  -  {item.Name} url : {item.Url}");

            result.IsAlive = CheckIsAlive(item.Url);
            NotifyUser(item, result);

            _healthCheckService.MarkAsChecked(new UpdateChecksStatusDto { CheckDate = DateTime.Now, IsAlive = result.IsAlive, Id = item.Id });

            return result;

        }

        private void NotifyUser(TargetAppDto item, AppCheckResultDto result)
        {
            if (!result.IsAlive)
            {
                var user = _userService.GetById(item.CreatedById.GetValueOrDefault(0));
                if (user == null) return;

                var notificationService = NotificationServiceFactory.GetNotificationService(user.NotificationPreference);
                notificationService.NotifyDown(user, item);
                result.NotifiedVia = user.NotificationPreference;
                result.IsUserNotified = true;
                result.UserId = user.Id;

            }
        }

        public virtual bool CheckIsAlive(string url)
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
