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

namespace HealthCheck.Web.Controllers
{
    public class AppManagerController : Controller
    {
        private readonly ILogger<AppManagerController> _logger;
        private readonly HealthCheckService _healthCheckService;

        public AppManagerController(ILogger<AppManagerController> logger, HealthCheckService healthCheckService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;

        }

        public IActionResult Index()
        {
            var all = _healthCheckService.All(new GetTargetAllAppDto { });
            return View(all);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTargetAppDto app)
        {
            var result =_healthCheckService.Add(app);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            _healthCheckService.Delete(new DeleteTargetAppDto { Id=Id });
            return RedirectToAction("Index");

        }

        public IActionResult Update(int Id)
        {
            var model = _healthCheckService.GetOne(new GetOneTargetAppDto { Id = Id });
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateTargetAppDto app)
        {
            _healthCheckService.Update(app);
            return RedirectToAction("Index");


        }
    }
}
