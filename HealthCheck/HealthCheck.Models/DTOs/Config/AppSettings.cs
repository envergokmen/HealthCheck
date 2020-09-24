using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Models.DTOs
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string AppDbConnectionString { get; set; }
    }

}
