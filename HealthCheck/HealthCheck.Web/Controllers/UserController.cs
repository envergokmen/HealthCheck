﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthCheck.Web.Models;
using HealthCheck.Models.DTOs.User;
using HealthCheck.Services;
using HealthCheck.Web.Membership;

namespace HealthCheck.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly MembershipService _membershipService;


        public UserController(ILogger<UserController> logger, MembershipService membershipService)
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
                return RedirectToAction("Index", "AppManager");
            }
            else
            {
                ModelState.AddModelError(string.Empty,"Invalid User Info");
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
            var loginResult = _membershipService.Register(registerModel);

            if (loginResult != null)
            {
                return RedirectToAction("Index", "AppManager");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration Error");
                return View(registerModel);
            }
        }


    }
}
