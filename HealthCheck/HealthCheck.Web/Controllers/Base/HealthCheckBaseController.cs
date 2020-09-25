using HealthCheck.Models.DTOs.User;
using HealthCheck.Web.Membership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HealthCheck.Web.Controllers
{
    public class HealthCheckBaseController : Controller
    {
        private readonly IMembershipService _membershipService;
        protected UserDto user;

        public HealthCheckBaseController(IMembershipService _userService)
        {
            this._membershipService = _userService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.user = _membershipService.GetUser();
            ViewBag.User = user;

            base.OnActionExecuting(context);
        }
    }
}
