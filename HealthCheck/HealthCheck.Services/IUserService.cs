using HealthCheck.Models;
using HealthCheck.Models.DTOs.User;

namespace HealthCheck.Services
{
    public interface IUserService
    {
        UserDto GetById(int Id);
        NotificationType GetUserNotificationType(int Id);
        UserDto Login(UserLoginDto login);
        UserDto Register(UserRegisterDto registerDto);
    }
}