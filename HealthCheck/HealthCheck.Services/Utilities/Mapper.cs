using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services.Utilities
{
    public class Mapper
    {
        public static TargetAppDto MapToAppDto(TargetApp app)
        {
            if (app == null) return null;

            return new TargetAppDto { IntervalType = app.IntervalType, IntervalValue = app.IntervalValue, CreatedById = app.CreatedById, Id = app.Id, Url = app.Url, Name = app.Name, IsAlive = app.IsAlive, LastCheck = app.LastCheck };

        }
        public static UpdateTargetAppDto MapToUpdateDto(TargetApp app)
        {
            if (app == null) return null;
            return new UpdateTargetAppDto { IntervalType = app.IntervalType, IntervalValue = app.IntervalValue,  Id = app.Id, Url = app.Url, Name = app.Name};

        }
        public static List<TargetAppDto> MapToAppDto(List<TargetApp> app)
        {
            if (app == null) return null;
            return app.Select(x => new TargetAppDto { IntervalType = x.IntervalType, IntervalValue = x.IntervalValue, CreatedById = x.CreatedById, Id = x.Id, Url = x.Url, Name = x.Name, IsAlive = x.IsAlive, LastCheck = x.LastCheck }).ToList();
        }

        public static  TargetApp MapToAppFromCreateDto(CreateTargetAppDto registerDto)
        {
            if (registerDto == null) return null;

            return new TargetApp { Name = registerDto.Name, Url = registerDto.Url, CreatedById = registerDto.LoggedInUserId, IntervalValue = registerDto.IntervalValue, IntervalType = registerDto.IntervalType };
        }

        public static void SetPropsFromDto(TargetApp app, UpdateTargetAppDto updateDto)
        {
            app.Name = updateDto.Name;
            app.Url = updateDto.Url;
            app.IntervalType = updateDto.IntervalType;
            app.IntervalValue = updateDto.IntervalValue;
            app.IsAlive = null;
        }

        public static UserDto MapToUserDto(User user)
        {
            if (user == null) return null;
            return new UserDto { Id = user.Id, UserName = user.Username, Name = user.Name, NotificationPreference = user.NotificationPreference, Email = user.Email, Gsm = user.Gsm };
        }

        public static User MapToUserFromRegister(UserRegisterDto registerDto, string secret)
        {
            return  new User { Email = registerDto.Email, Gsm = registerDto.Gsm, Username = registerDto.Username, Name = registerDto.Name, Password = SecurityUtils.Encrypt(registerDto.Password, secret) };

        }
    }
}
