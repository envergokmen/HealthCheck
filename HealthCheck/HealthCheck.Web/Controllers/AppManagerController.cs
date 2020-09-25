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
using HealthCheck.Services.Extensions;

namespace HealthCheck.Web.Controllers
{
    [PolicyBasedAuthorize(HealthCheck.Models.Policy.Manager)]
    public class AppManagerController : HealthCheckBaseController
    {
        private readonly ILogger<AppManagerController> _logger;
        private readonly ITargetAppService _targetAppService;
        private readonly IMembershipService _memeberShipService;

        public AppManagerController(ILogger<AppManagerController> logger, ITargetAppService healthCheckService, IMembershipService userService) : base(userService)
        {
            _logger = logger;
            _targetAppService = healthCheckService;
            _memeberShipService = userService;
        }


        public IActionResult Index()
        {
            AppManagerIndexVM model = new AppManagerIndexVM(user);
            model.Apps = _targetAppService.All(new GetTargetAllAppDto { LoggedInUserId = user.Id });

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
            model.Apps = _targetAppService.All(new GetTargetAllAppDto { LoggedInUserId = user.Id });

            return PartialView("Index", model);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTargetAppDto app)
        {

            if (!app.Url.IsValidUrl())
            {
                ModelState.AddModelError(string.Empty, "Invalid Url Please Check");
                return View(app);
            }

            app.LoggedInUserId = user.Id;
            _targetAppService.Add(app);


            return RedirectToAction("Index");
        }


        //to easy test sample apps.
        public IActionResult CreateBulk()
        {

            var testData = new List<CreateTargetAppDto>
            {
                new CreateTargetAppDto {  Name = "google", Url = "https://www.google.com", LoggedInUserId = user.Id, IntervalType = IntervalType.Minutely, IntervalValue = 1 },
                new CreateTargetAppDto {   Name = "twitter", Url = "https://www.twitter.com", LoggedInUserId = user.Id, IntervalType = IntervalType.Minutely, IntervalValue = 1 },
                new CreateTargetAppDto {   Name = "facebook", Url = "https://www.facebook.com", LoggedInUserId = user.Id, IntervalType = IntervalType.Minutely, IntervalValue = 1 },
                new CreateTargetAppDto {   Name = "http 500 test", Url = "https://httpstat.us/500", LoggedInUserId = user.Id, IntervalType = IntervalType.Minutely, IntervalValue = 1 }
            };

            foreach (var app in testData)
            {
                app.LoggedInUserId = user.Id;
                _targetAppService.Add(app);
            }

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int Id)
        {

            _targetAppService.Delete(new DeleteTargetAppDto { Id = Id, LoggedInUserId = user.Id });
            RecurringJob.RemoveIfExists("site-healthcheck-" + Id.ToString());

            if (Request.IsAjaxRequest())
            {
                return Content("true");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update(int Id)
        {
            var model = _targetAppService.GetOne(new GetOneTargetAppDto { Id = Id, LoggedInUserId = user.Id });

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateTargetAppDto app)
        {

            if (!app.Url.IsValidUrl())
            {
                ModelState.AddModelError(string.Empty, "Invalid Url Please Check");
                return View(app);
            }

            app.LoggedInUserId = user.Id;
            _targetAppService.Update(app);

            return RedirectToAction("Index");

        }

    }
}
