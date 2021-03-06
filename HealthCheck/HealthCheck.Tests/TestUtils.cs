﻿using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs;
using Microsoft.Extensions.Options;

namespace HealthCheck.Tests
{
    public class TestUtils
    {
        public static void AddSeedTestData(HealthContext context)
        {
            context.Users.Add(new User { Id = 1, Username = "enver", Name = "Enver Gökmen", Email = "envergokmen@gmail.com", Gsm = "5069516750", NotificationPreference= NotificationType.Email });
            context.Users.Add(new User { Id = 2, Username = "diger", Name = "Diger Kullanıcı", Email = "vectorman87@gmail.com", Gsm = "5069516750", NotificationPreference = NotificationType.Sms });

            //user1's apps
            context.TargetApps.Add(new TargetApp { Id = 1, Name = "google", Url = "https://www.google.com", CreatedById = 1, IntervalType = IntervalType.Minutely, IntervalValue = 1 });
            context.TargetApps.Add(new TargetApp { Id = 2, Name = "twitter", Url = "https://www.twitter.com", CreatedById = 1, IntervalType = IntervalType.Minutely, IntervalValue = 1 });

            //users 2's apps
            context.TargetApps.Add(new TargetApp { Id = 3, Name = "facebook", Url = "https://www.facebook.com", CreatedById = 2, IntervalType = IntervalType.Minutely, IntervalValue = 1 });

            context.SaveChanges();
        }

    }
    public class MockedAppSetting : IOptions<AppSettings>
    {
        public AppSettings Value => new AppSettings { Secret = "ENVDEMOKEY2020" };
    }
}
