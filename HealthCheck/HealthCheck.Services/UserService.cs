using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs;
using HealthCheck.Models.DTOs.User;
using HealthCheck.Services.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services
{
    public class UserService
    {
        private readonly HealthContext db;
        protected readonly AppSettings appSettings;

        public UserService(HealthContext _db, IOptions<AppSettings> _appSettings)
        {
            db = _db;
            this.appSettings = _appSettings.Value;
        }

        public UserDto Login(UserLoginDto login)
        {
            return db.Users.Where(x => x.Username == login.Username && x.Password == SecurityUtils.Encrypt(login.Password, appSettings.Secret)).Select(x => new UserDto { Id = x.Id, UserName = x.Username, Name = x.Name, NotificationPreference = x.NotificationPreference }).FirstOrDefault();
        }

        public UserDto GetById(int Id)
        {
            return db.Users.Where(x => x.Id == Id).Select(x => new UserDto { Id = x.Id, UserName = x.Username, Name = x.Name, NotificationPreference = x.NotificationPreference }).FirstOrDefault();
        }

        public NotificationType GetUserNotificationType(int Id)
        {
            return db.Users.Where(x => x.Id == Id).Select(x => x.NotificationPreference).FirstOrDefault();
        }

        public UserDto Register(UserRegisterDto registerDto)
        {
            var user = db.Users.Where(x => x.Username == registerDto.Username).FirstOrDefault();

            if (user != null) { throw new Exception("This username in use");  };

            user = new User { Email=registerDto.Email, Gsm= registerDto.Gsm, Username = registerDto.Username, Name = registerDto.Name, Password = SecurityUtils.Encrypt(registerDto.Password, appSettings.Secret) };
            db.Users.Add(user);
            db.SaveChanges();

            return new UserDto { Id = user.Id, UserName = user.Username, Name = user.Name, NotificationPreference = user.NotificationPreference };

        }
    }
}
