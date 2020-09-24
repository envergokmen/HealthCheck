using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Models.DTOs.User;

namespace HealthCheck.Services
{
    public interface INotificationService
    {
        void NotifyDown(UserDto user, TargetAppDto targetApp);
    }
}