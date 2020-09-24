using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs;
using HealthCheck.Services;
using HealthCheck.Web.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HealthCheck.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<HealthContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HealthDbCon")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserService, UserService>();
            services.AddTransient<MembershipService, MembershipService>();
            services.AddTransient<HealthCheckService, HealthCheckService>();
            services.AddScoped<IBackgroundHangService, BackgroundHangService>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            services.AddHangfire(x => x.UseSqlServerStorage("Server=DESKTOP-UP8JB10;Database=HangFire2;user id=sa; password=123456;MultipleActiveResultSets=true"));
            services.AddHangfireServer();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
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
