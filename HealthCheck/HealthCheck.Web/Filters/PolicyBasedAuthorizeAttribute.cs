using HealthCheck.Models;
using HealthCheck.Web.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Web.Filters
{
    public class PolicyBasedAuthorizeAttribute : TypeFilterAttribute
    {
        public PolicyBasedAuthorizeAttribute(Policy policy) : base(typeof(PolicyBasedAuthorize))
        {
            Arguments = new object[] { policy.ToString() };
        }
    };

    public class PolicyBasedAuthorize : IAuthorizationFilter
    {
        readonly string policy;

        readonly IAuthorizationService _authorizationService;
        readonly IMembershipService _membershipService;

        public PolicyBasedAuthorize(string policy, IAuthorizationService authorizationService, IMembershipService membershipService)
        {
            this.policy = policy;
            _authorizationService = authorizationService;
            _membershipService = membershipService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if(_membershipService.GetUser()!=null)
            {
                return;
            }

            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            string returnType = controllerActionDescriptor.MethodInfo.ReturnType.Name;

            if (returnType == nameof(JsonResult))
            {
                
                context.Result = new JsonResult(false);
            }
            else
            {
                context.Result = new RedirectResult("/User/Login");
            }
        }
    }
}
