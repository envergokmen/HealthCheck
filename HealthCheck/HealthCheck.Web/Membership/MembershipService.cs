using HealthCheck.Models.DTOs.User;
using HealthCheck.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Web.Membership
{
    public class MembershipService : IMembershipService
    {
        private readonly HttpContext httpContext;
        private readonly IUserService _userService;

        public MembershipService(IHttpContextAccessor contextAccessor, IUserService userService)
        {
            httpContext = contextAccessor?.HttpContext;
            _userService = userService;
        }

        public void SetUser(UserDto user)
        {
            if (user == null) throw new Exception("User cannot be null");
            httpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
        }

        public UserDto GetUser()
        {
            UserDto usr = null;

            if (httpContext.Session != null && httpContext.Session.GetString("User") != null)
            {
                var value = httpContext.Session.GetString("User");
                return value == null ? null : JsonConvert.DeserializeObject<UserDto>(value);
            }

            return usr;
        }

        public UserDto Login(UserLoginDto login)
        {
            return _userService.Login(login);
        }

        public void Logout()
        {
            httpContext.Session.Remove("User");
        }

        public UserDto Register(UserRegisterDto registerDto)
        {
            return _userService.Register(registerDto);

        }

    }
}
