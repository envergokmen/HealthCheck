using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services;
using Microsoft.Extensions.Hosting;

namespace HealthCheck.Web
{
    public class ApplicationBackgroundService : IHostedService, IDisposable
    {
        private readonly HealthCheckService _healthCheckService;
        private Timer _timer;

        public ApplicationBackgroundService()
        {
            _healthCheckService = new HealthCheckService(new Database.HealthContext());
        }

        public void DispatchQueue(object state = null)
        {
             var queueToProcess = _healthCheckService.GetOneToCheck();
             ProcessQueueItem(queueToProcess);
        }

        private async Task ProcessQueueItem(UpdateTargetAppDto item)
        {
             if (item == null) return;
        
            Debug.WriteLine($"Processing {item.Id}  -  {item.Name} url : {item.Url}");

            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://www.chalkartproject.com/");
            var _isAlive = response.StatusCode == System.Net.HttpStatusCode.OK ? true : false;

            //to do check from http request.
            _healthCheckService.MarkAsChecked(new UpdateChecksStatusDto { CheckDate=DateTime.Now, IsAlive= _isAlive, Id=item.Id });

        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DispatchQueue, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }
      
        public Task StopAsync(CancellationToken stoppingToken)
        {

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public virtual void Dispose(bool disposing)
        {
            _timer?.Dispose();
        }

        public void Dispose()
        {
            _timer?.Dispose();

        }
    }
}
