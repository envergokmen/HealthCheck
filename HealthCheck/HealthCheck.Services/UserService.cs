using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services
{
    public class UserService
    {
        private readonly HealthContext db;

        public UserService(HealthContext _db)
        {
            db = _db;
        }

        public UserDto Login(UserLoginDto login)
        {
            return db.Users.Where(x => x.Username == login.Username).Select(x => new UserDto { Id = x.Id, UserName = x.Username, Name = x.Name }).FirstOrDefault();
        }

        public UserDto Register(UserRegisterDto registerDto)
        {
            var user = new User { Username = registerDto.Username, Name = registerDto.Name };
            db.Users.Add(user);
            db.SaveChanges();

            return new UserDto { Id = user.Id, UserName = user.Username, Name = user.Name };

        }
    }
}
