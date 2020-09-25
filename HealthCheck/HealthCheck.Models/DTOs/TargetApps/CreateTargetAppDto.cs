using System.ComponentModel.DataAnnotations;

namespace HealthCheck.Models.DTOs.TargetApps
{
    public class CreateTargetAppDto : BaseRequestDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Url { get; set; }

        [Required]
        public IntervalType IntervalType { get; set; }

        [Required]
        [Range(1, 364)]
        public int IntervalValue { get; set; }
    }
}
