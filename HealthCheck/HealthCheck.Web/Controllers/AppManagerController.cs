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
using HealthCheck.Services.Utilities;

namespace HealthCheck.Web.Controllers
{
    [PolicyBasedAuthorize(HealthCheck.Models.Policy.Manager)]
    public class AppManagerController : HealthCheckBaseController
    {
        private readonly ILogger<AppManagerController> _logger;
        private readonly ITargetAppService _healthCheckService;
        private readonly IMembershipService _memeberShipService;
        private readonly IBackgroundHealthCheckerService _bgCheckService;


        public AppManagerController(ILogger<AppManagerController> logger, ITargetAppService healthCheckService, IMembershipService userService, IBackgroundHealthCheckerService bgCheckService) : base(userService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
            _memeberShipService = userService;
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
            var user = _memeberShipService.GetUser();
          
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

            RecurringJob.AddOrUpdate("site-healthcheck-" + created.Id.ToString() , methodCall: () => _bgCheckService.CheckDownOrAlive(created), cronExpression: CronUtils.GetCronExpression(app.IntervalType, app.IntervalValue));
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

            RecurringJob.AddOrUpdate("site-healthcheck-" + updated.Id.ToString(), () => _bgCheckService.CheckDownOrAlive(updated), CronUtils.GetCronExpression(app.IntervalType, app.IntervalValue));

            
            return RedirectToAction("Index");

        }

    }
}
