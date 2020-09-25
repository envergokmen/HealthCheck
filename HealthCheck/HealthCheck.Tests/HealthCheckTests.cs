using HealthCheck.Database;
using HealthCheck.Models.DTOs;
using HealthCheck.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HealthCheck.Tests
{
    [TestClass]
    public class HealthCheckTests
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
                var jobScheduler = new MockedSchedulerService();
                var appService = new TargetAppService(db, jobScheduler);
                var mockBgService = new MockedBackgroundService(db, userService);

                var itemToCheck = appService.GetOneToCheck(1);
                mockBgService.CheckDownOrAlive(itemToCheck);

                itemToCheck = appService.GetOneToCheck(1);

                Assert.IsNotNull(itemToCheck.IsAlive, "checking health is not done");

            }
        }

        [TestMethod]
        public void Correct_User_Should_Be_Notifited_According_To_Its_Notification_Preference()
        {
            var options = new DbContextOptionsBuilder<HealthContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString() + "HealthCheckTestDB").Options;

            using (var db = new HealthContext(options))
            {
                TestUtils.AddSeedTestData(db);
                IOptions<AppSettings> appsettings = new MockedAppSetting();

                var userService = new UserService(db, appsettings);

                var curUser = userService.GetById(1); //Email Notification preference by mocked data
                var jobScheduler = new MockedSchedulerService();
                var appService = new TargetAppService(db, jobScheduler);

                var mockBgService = new MockedBackgroundService(db, userService);

                var itemToCheck = appService.GetOneToCheck(2, curUser.Id); //app 2 is down mocked
                var checkUrlResult = mockBgService.CheckDownOrAlive(itemToCheck);

                Assert.AreEqual(checkUrlResult.UserId, curUser.Id, "Users are not the same");
                Assert.IsTrue(checkUrlResult.IsUserNotified, "Users is not Notified");
                Assert.IsNotNull(checkUrlResult.NotifiedVia, "Users is not notified (NotifiedVia is null)");
                Assert.AreEqual(curUser.NotificationPreference, checkUrlResult.NotifiedVia.Value, "Users's preference was different than notification");

                //Check for another Notification preferance for another user, SMS Notification preference by mocked data for user 2
                curUser = userService.GetById(2);
               
                itemToCheck = appService.GetOneToCheck(3, curUser.Id); //app 3 is down mocked
                checkUrlResult = mockBgService.CheckDownOrAlive(itemToCheck);

                Assert.AreEqual(checkUrlResult.UserId, curUser.Id, "Users are not the same");
                Assert.IsTrue(checkUrlResult.IsUserNotified, "Users is not Notified");
                Assert.IsNotNull(checkUrlResult.NotifiedVia, "Users is not notified (NotifiedVia is null)");
                Assert.AreEqual(curUser.NotificationPreference, checkUrlResult.NotifiedVia.Value, "Users's preference was different than notification");

            }
        }

        [TestMethod]
        public void User_Should_Not_Be_Notified_If_App_Is_Alive()
        {
            var options = new DbContextOptionsBuilder<HealthContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString() + "HealthCheckTestDB").Options;

            using (var db = new HealthContext(options))
            {
                TestUtils.AddSeedTestData(db);
                IOptions<AppSettings> appsettings = new MockedAppSetting();

                var userService = new UserService(db, appsettings);

                var curUser = userService.GetById(1);

                var jobScheduler = new MockedSchedulerService();
                var appService = new TargetAppService(db, jobScheduler);

                var mockBgService = new MockedBackgroundService(db, userService);

                var itemToCheck = appService.GetOneToCheck(1, curUser.Id); //1 is alive mocked
                var checkUrlResult = mockBgService.CheckDownOrAlive(itemToCheck);

                Assert.IsFalse(checkUrlResult.IsUserNotified, "Opps, Users is notified for the alive app");


            }
        }
    }


}
