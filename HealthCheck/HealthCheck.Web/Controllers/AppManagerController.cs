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

namespace HealthCheck.Web.Controllers
{
    public class AppManagerController : Controller
    {
        private readonly ILogger<AppManagerController> _logger;
        private readonly HealthCheckService _healthCheckService;
        private readonly MembershipService _userService;


        public AppManagerController(ILogger<AppManagerController> logger, HealthCheckService healthCheckService, MembershipService userService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
            _userService = userService;

        }

        public IActionResult Index()
        {
            var user = _userService.GetUser();
            if (user == null) return RedirectToAction("Login", "User");

            ViewBag.User = user;

            AppManagerIndexVM model = new AppManagerIndexVM(user);

            if (user != null)
            {
                model.Apps = _healthCheckService.All(new GetTargetAllAppDto { LoggedInUserId = user.Id });
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public IActionResult GetStatuses()
        {
            var user = _userService.GetUser();
            if (user == null) return RedirectToAction("Login", "User");

            ViewBag.User = user;

            AppManagerIndexVM model = new AppManagerIndexVM(user);

            if (user != null)
            {
                model.Apps = _healthCheckService.All(new GetTargetAllAppDto { LoggedInUserId = user.Id });
            }

            return PartialView("Index", model);

        }

        public IActionResult Create()
        {
            var user = _userService.GetUser();
            if (user == null) return RedirectToAction("Login", "User");
            ViewBag.User = user;

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTargetAppDto app)
        {
            var user = _userService.GetUser();
            if (user == null) return RedirectToAction("Login", "User");
            ViewBag.User = user;

            app.LoggedInUserId = user.Id;

            var result = _healthCheckService.Add(app);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var user = _userService.GetUser();
            if (user == null) return RedirectToAction("Login", "User");
            ViewBag.User = user;

            _healthCheckService.Delete(new DeleteTargetAppDto { Id = Id, LoggedInUserId = user.Id });

            if (Request.IsAjaxRequest())
            {
                return Content("true");
            }

            return RedirectToAction("Index");

        }

        public IActionResult Update(int Id)
        {
            var user = _userService.GetUser();
            if (user == null) return RedirectToAction("Login", "User");
            ViewBag.User = user;

            var model = _healthCheckService.GetOne(new GetOneTargetAppDto { Id = Id, LoggedInUserId = user.Id });
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateTargetAppDto app)
        {
            var user = _userService.GetUser();
            app.LoggedInUserId = user.Id;
            ViewBag.User = user;

            _healthCheckService.Update(app);
            return RedirectToAction("Index");


        }
    }
}
