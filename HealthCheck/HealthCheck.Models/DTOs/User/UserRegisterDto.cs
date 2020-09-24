using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCheck.Models.DTOs.User
{
    public class UserRegisterDto
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(60)]
        public string Username { get; set; }

        [StringLength(90)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Gsm { get; set; }

        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(30)]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }


    }
}
