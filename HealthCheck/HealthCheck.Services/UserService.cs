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
    public class UserService : IUserService
    {
        private readonly HealthContext _db;
        protected readonly AppSettings _appSettings;

        public UserService(HealthContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }

        public UserDto Login(UserLoginDto login)
        {
            var user= _db.Users.Where(x => x.Username == login.Username && x.Password == SecurityUtils.Encrypt(login.Password, _appSettings.Secret)).FirstOrDefault();
            return Mapper.MapToUserDto(user);
        }

        public UserDto GetById(int Id)
        {
            var user= _db.Users.Where(x => x.Id == Id).FirstOrDefault();
            return Mapper.MapToUserDto(user);
        }

        public NotificationType GetUserNotificationType(int Id)
        {
            return _db.Users.Where(x => x.Id == Id).Select(x => x.NotificationPreference).FirstOrDefault();
        }

        public UserDto Register(UserRegisterDto registerDto)
        {
            var user = _db.Users.Where(x => x.Username == registerDto.Username).FirstOrDefault();
            if (user != null) { throw new Exception("This username in use"); };

            user = Mapper.MapToUserFromRegister(registerDto, _appSettings.Secret);
            _db.Users.Add(user);
            _db.SaveChanges();

            return Mapper.MapToUserDto(user); 

        }
    }
}
