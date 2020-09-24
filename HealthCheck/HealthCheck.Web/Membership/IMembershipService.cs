using HealthCheck.Models.DTOs.User;

namespace HealthCheck.Web.Membership
{
    public interface IMembershipService
    {
        UserDto GetUser();
        UserDto Login(UserLoginDto login);
        void Logout();
        UserDto Register(UserRegisterDto registerDto);
        void SetUser(UserDto user);
    }
}