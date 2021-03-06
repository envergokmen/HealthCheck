﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthCheck.Models.DTOs.User;
using HealthCheck.Web.Membership;

namespace HealthCheck.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMembershipService _membershipService;


        public UserController(ILogger<UserController> logger, IMembershipService membershipService)
        {
            _logger = logger;
            _membershipService = membershipService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginDto loginModel)
        {
            var loginResult = _membershipService.Login(loginModel);

            if (loginResult != null)
            {
                _membershipService.SetUser(loginResult);
                return RedirectToAction("Index", "AppManager");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid User Info");
                return View(loginModel);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterDto registerModel)
        {
            var registerResult = _membershipService.Register(registerModel);

            if (registerResult != null)
            {
                _membershipService.SetUser(registerResult);

                return RedirectToAction("Index", "AppManager");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration Error");
                return View(registerModel);
            }
        }

        public IActionResult Logout()
        {
            _membershipService.Logout();
            return RedirectToAction("Login", "User");
        }

        public IActionResult ErrorTest()
        {
            _logger.LogError("test ErrorTest user");
            return Content("OK");
        }

        public IActionResult Error2()
        {
            var b = 10;
            var c = 0;
            var x = b / c;

            return Content("OK");
        }

    }
}
