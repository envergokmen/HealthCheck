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
    public class BackgroundHealthCheckService : IHostedService, IDisposable
    {
        public readonly HealthCheckService _healthCheckService;
        private Timer _timer;

        public BackgroundHealthCheckService()
        {
            _healthCheckService = new HealthCheckService(new Database.HealthContext());
        }

        public async void DispatchQueue(object state = null)
        {
             var queueToProcess = _healthCheckService.GetOneToCheck();
             await ProcessQueueItem(queueToProcess);
        }

        private async Task ProcessQueueItem(UpdateTargetAppDto item)
        {
            if (item == null) return;

            Debug.WriteLine($"Processing {item.Id}  -  {item.Name} url : {item.Url}");

           //var isAlive = await CheckIsAlive(item.Url);
            //_healthCheckService.MarkAsChecked(new UpdateChecksStatusDto { CheckDate = DateTime.Now, IsAlive = isAlive, Id = item.Id });

        }

        private static async Task<bool> CheckIsAlive(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var response = await httpClient.GetAsync(url);
                return response.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
            }

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
