using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthCheck.Web.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services;
using HealthCheck.Web.Membership;
using HealthCheck.Models.DTOs.ViewModels;
using HealthCheck.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using HealthCheck.Web.Filters;
using Hangfire;
using HealthCheck.Models;

namespace HealthCheck.Web.Controllers
{
    [PolicyBasedAuthorize(HealthCheck.Models.Policy.Manager)]
    public class AppManagerController : HealthCheckBaseController
    {
        private readonly ILogger<AppManagerController> _logger;
        private readonly HealthCheckService _healthCheckService;
        private readonly MembershipService _userService;
        private readonly IBackgroundHangService _bgCheckService;


        public AppManagerController(ILogger<AppManagerController> logger, HealthCheckService healthCheckService, MembershipService userService, IBackgroundHangService bgCheckService) : base(userService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
            _userService = userService;
            _bgCheckService = bgCheckService;
        }


        public IActionResult Index()
        {
            AppManagerIndexVM model = new AppManagerIndexVM(user);
            model.Apps = _healthCheckService.All(new GetTargetAllAppDto { LoggedInUserId = user.Id });

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public IActionResult GetStatuses()
        {
            var user = _userService.GetUser();
          
            AppManagerIndexVM model = new AppManagerIndexVM(user);
            model.Apps = _healthCheckService.All(new GetTargetAllAppDto { LoggedInUserId = user.Id });

            return PartialView("Index", model);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTargetAppDto app)
        {

            app.LoggedInUserId = user.Id;
            var created = _healthCheckService.Add(app);

            RecurringJob.AddOrUpdate("site-healthcheck-" + created.Id.ToString() , methodCall: () => _bgCheckService.CheckDownOrAlive(created), cronExpression: GetCronExpression(app.IntervalType, app.IntervalValue));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
           
            _healthCheckService.Delete(new DeleteTargetAppDto { Id = Id, LoggedInUserId = user.Id });
            RecurringJob.RemoveIfExists("site-healthcheck-" + Id.ToString());

            if (Request.IsAjaxRequest())
            {
                return Content("true");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update(int Id)
        {
            var model = _healthCheckService.GetOne(new GetOneTargetAppDto { Id = Id, LoggedInUserId = user.Id });

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateTargetAppDto app)
        {
            app.LoggedInUserId = user.Id;
            var updated = _healthCheckService.Update(app);

            RecurringJob.AddOrUpdate("site-healthcheck-" + updated.Id.ToString(), () => _bgCheckService.CheckDownOrAlive(updated), GetCronExpression(app.IntervalType, app.IntervalValue));

            
            return RedirectToAction("Index");

        }

        public string GetCronExpression(IntervalType intervalType, int value)
        {
            switch (intervalType)
            {
                case IntervalType.Minutely:
                    return Cron.MinuteInterval(value);
                    break;
                case IntervalType.Hourly:
                    return Cron.HourInterval(value);
                    break;
                case IntervalType.Daily:
                    return Cron.DayInterval(value);
                    break;
                case IntervalType.Monthly:
                    return Cron.MonthInterval(value);
                    break;
            }

            throw new Exception("please select a valid interval type");
        }
    }
}
