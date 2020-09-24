using HealthCheck.Models.DTOs.TargetApps;

namespace HealthCheck.Web
{
    public interface IBackgroundHangService
    {
        void CheckDownOrAlive(TargetAppDto item);
    }
}