namespace HealthCheck.Models.DTOs.TargetApps
{
    public class AppCheckResultDto
    {
        public int AppId { get; set; }
        public int UserId { get; set; }
        public bool IsAlive { get; set; }
        public bool IsUserNotified { get; set; }
        public NotificationType? NotifiedVia { get; set; }
    }
}
