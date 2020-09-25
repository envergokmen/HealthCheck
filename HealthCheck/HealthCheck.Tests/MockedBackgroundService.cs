using HealthCheck.Database;
using HealthCheck.Services;
using HealthCheck.Web;

namespace HealthCheck.Tests
{
    public class MockedBackgroundService : BgHealthCheckingService
    {

        public MockedBackgroundService(HealthContext db, IUserService userService) : base(db, userService)
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
