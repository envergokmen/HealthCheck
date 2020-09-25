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
    public class UserServiceTest
    {
        [TestMethod]
        public void Userserivce_Should_NotRegister_Duplicate_Users()
        {
            var options = new DbContextOptionsBuilder<HealthContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString() + "HealthCheckTestDB").Options;

            try
            {
                using (var db = new HealthContext(options))
                {
                    TestUtils.AddSeedTestData(db);
                    IOptions<AppSettings> appsettings = new MockedAppSetting();

                    var userService = new UserService(db, appsettings);

                    userService.Register(new Models.DTOs.User.UserRegisterDto { Username = "enver", Name = "Enver Gökmen", Email = "envergokmen@gmail.com", Gsm = "5069516750", Password = "123123" });
                }

                Assert.Fail(); // raises AssertionException
            }
            catch (Exception ex)
            {
                Assert.AreEqual("This username in use", ex.Message);
            }

        }

    }
}
