using System;
using Hangfire;
using HealthCheck.Database;
using HealthCheck.Models.DTOs;
using HealthCheck.Services;
using HealthCheck.Web.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;

namespace HealthCheck.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<HealthContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HealthDbCon")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<ITargetAppService, TargetAppService>();
            services.AddScoped<IBackgroundHealthCheckerService, BgHealthCheckingService>();
            services.AddScoped<IJobScheduler, JobScheduler>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HealthDbCon")));
            services.AddHangfireServer();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorization();

            services.AddLogging(
              builder =>
              {
                  builder.AddFilter("Microsoft", LogLevel.Warning)
                         .AddFilter("System", LogLevel.Warning)
                         .AddFilter("NToastNotify", LogLevel.Warning)
                         .AddFile("ErrorLogs.json");
              });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging(options => new RequestLoggingOptions
            {


            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseHangfireDashboard();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=AppManager}/{action=Index}/{id?}");
            });
        }
    }
}
