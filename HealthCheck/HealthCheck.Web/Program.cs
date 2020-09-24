using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace HealthCheck.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Log.Logger = new LoggerConfiguration()
            //.Enrich.FromLogContext()
            //.WriteTo.Console()
            //.CreateLogger();
            Log.Logger = new LoggerConfiguration()

                .MinimumLevel.Error()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("ErrorLogs.json")
                .CreateLogger();

            Log.Error("ERROR Health Check is Starting...");
            Log.Information("INFO Health Check is Starting...");


            try
            {
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception e)
            {
                Log.Error(e, "@e");
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(config => config.ClearProviders()).UseSerilog();
    }
}
