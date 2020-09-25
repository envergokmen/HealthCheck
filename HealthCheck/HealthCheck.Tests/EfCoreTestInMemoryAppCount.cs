using HealthCheck.Database;
using HealthCheck.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HealthCheck.Tests
{
    [TestClass]
    public class EfCoreTestInMemoryAppCount
    {
        [TestMethod]
        public void TestEfInMemoryAppCountForUser()
        {
            var options = new DbContextOptionsBuilder<HealthContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString() + "HealthCheckTestDB") .Options;

            using (var context = new HealthContext(options))
            {
                AddSeedTestData(context);

                context.SaveChanges();

                var count = context.TargetApps.Where(x => x.CreatedById == 1).Count();
                Assert.AreEqual(2, count, "Kullnıcı 1'in 2 app'i olması gerekir.");
            }

        }

        private static void AddSeedTestData(HealthContext context)
        {
            context.Users.Add(new User { Id = 1, Name = "Enver Gökmen", Email = "envergokmen@gmail.com", Gsm = "5069516750", NotificationPreference= NotificationType.Email });
            context.Users.Add(new User { Id = 2, Name = "Diger Kullanıcı", Email = "vectorman87@gmail.com", Gsm = "055555555", NotificationPreference = NotificationType.Sms });

            context.TargetApps.Add(new TargetApp { Id = 1, Name = "google", Url = "https://www.google.com", CreatedById = 1, IntervalType = IntervalType.Minutely, IntervalValue = 1 });
            context.TargetApps.Add(new TargetApp { Id = 2, Name = "google", Url = "https://www.twitter.com", CreatedById = 1, IntervalType = IntervalType.Minutely, IntervalValue = 1 });

            context.TargetApps.Add(new TargetApp { Id = 3, Name = "google", Url = "https://www.facebook.com", CreatedById = 2, IntervalType = IntervalType.Minutely, IntervalValue = 1 });
        }
    }
}
