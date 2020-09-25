using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs;
using HealthCheck.Services;
using HealthCheck.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthCheck.Tests
{
    [TestClass]
    public class TargetAppServiceTest
    {
        [TestMethod]
        public void Health_Checking_Should_Update_Apps_Alive_Status()
        {
            var options = new DbContextOptionsBuilder<HealthContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString() + "HealthCheckTestDB").Options;

            using (var db = new HealthContext(options))
            {
                TestUtils.AddSeedTestData(db);
                IOptions<AppSettings> appsettings = new MockedAppSetting();

                var userService = new UserService(db, appsettings);
                var appService = new TargetAppService(db);
                var mockBgService = new MockedBackgroundService(userService, appService);

                var itemToCheck = appService.GetOneToCheck(1);
                mockBgService.CheckDownOrAlive(itemToCheck);

                itemToCheck = appService.GetOneToCheck(1);

                Assert.IsNotNull(itemToCheck.IsAlive, "checking health is not done");

            }
        }

    }

    public class MockedBackgroundService : BgHealthCheckingService
    {
        public MockedBackgroundService(IUserService userService, ITargetAppService targetAppService) : base(userService, targetAppService)
        {

        }

        public override bool CheckIsAlive(string url)
        {
            switch (url)
            {
                case "https://www.google.com": return true; break;
                case "https://www.twitter.com": return false; break;
                default: return false; break;
            }
        }
    }
}
