using HealthCheck.Models.DTOs.User;
using HealthCheck.Web.Membership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Web.Controllers
{
    public class HealthCheckBaseController : Controller
    {
        private readonly MembershipService _userService;
        protected UserDto user;

        public HealthCheckBaseController(MembershipService _userService)
        {
            this._userService = _userService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.user = _userService.GetUser();
            ViewBag.User = user;

            base.OnActionExecuting(context);
        }
    }
}
