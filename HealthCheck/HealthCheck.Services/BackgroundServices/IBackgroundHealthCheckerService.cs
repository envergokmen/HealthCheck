using HealthCheck.Models.DTOs.TargetApps;

namespace HealthCheck.Web
{
    public interface IBackgroundHealthCheckerService
    {
        void CheckDownOrAlive(TargetAppDto item);
    }
}