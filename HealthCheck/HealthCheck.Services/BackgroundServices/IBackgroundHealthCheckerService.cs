using HealthCheck.Models.DTOs.TargetApps;

namespace HealthCheck.Web
{
    public interface IBackgroundHealthCheckerService
    {
        AppCheckResultDto CheckDownOrAlive(TargetAppDto item);
    }
}