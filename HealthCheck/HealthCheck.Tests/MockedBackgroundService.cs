using HealthCheck.Services;
using HealthCheck.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Tests
{
    public class MockedBackgroundService : BgHealthCheckingService
    {
        public MockedBackgroundService(IUserService userService, ITargetAppService targetAppService) : base(userService, targetAppService)
        {

        }

        public override bool CheckIsAlive(string url)
        {
            switch (url)
            {
                case "https://www.google.com": return true; 
                case "https://www.twitter.com": return false; 
                case "https://www.facebook.com": return false; 
                default: return false;
            }
        }
    }
}
