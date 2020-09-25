using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HealthCheck.Database;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services;
using HealthCheck.Services.NotificationServices;

namespace HealthCheck.Web
{

    public class BgHealthCheckingService : IBackgroundHealthCheckerService
    {
        public readonly IUserService _userService;
        public readonly HealthContext _db;

        public BgHealthCheckingService(HealthContext db, IUserService userService)
        {
            _userService = userService;
            _db = db;
        }

        public AppCheckResultDto CheckDownOrAlive(TargetAppDto item)
        {
            if (item == null) return null;
            AppCheckResultDto result = new AppCheckResultDto();

            //left on purpos in order to track
            Debug.WriteLine($"Processing {item.Id}  -  {item.Name} url : {item.Url}");

            result.IsAlive = CheckIsAlive(item.Url);
            NotifyUser(item, result);

            MarkAsChecked(new UpdateChecksStatusDto { CheckDate = DateTime.Now, IsAlive = result.IsAlive, Id = item.Id });

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

        public TargetAppDto MarkAsChecked(UpdateChecksStatusDto checkStatus)
        {
            var app = _db.TargetApps.FirstOrDefault(c => c.Id == checkStatus.Id);

            if (app == null) return null;

            app.IsAlive = checkStatus.IsAlive;
            app.LastCheck = checkStatus.CheckDate;

            _db.SaveChanges();

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name };
        }

        public virtual bool CheckIsAlive(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(5);
                    var response = httpClient.GetAsync(url).Result;
                    var statusCodeNumber = (int)response.StatusCode;
                    return statusCodeNumber >= 200 && statusCodeNumber < 300 ? true : false;
                }

            }
            catch (HttpRequestException)
            {
                //could be not marked different http status errors like
                return false;

            }
            catch (Exception)
            {
                //could be not marked different status like network error, timeout, etic.
                return false;
            }
        }

    }
}
