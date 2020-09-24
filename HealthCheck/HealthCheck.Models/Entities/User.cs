using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HealthCheck.Models
{
    public class User : DbBaseObject
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(60)]
        public string Username { get; set; }

        [StringLength(90)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [StringLength(20)]
        [Required]
        public string Gsm { get; set; }

        [StringLength(30)]
        public string Password { get; set; }

        public NotificationType NotificationPreference { get; set; }

    }
}
