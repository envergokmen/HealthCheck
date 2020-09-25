using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Services.Extensions
{
    public static class IsValidUrlExt
    {
        public static bool IsValidUrl(this string urlString)
        {
            try
            {
                new Uri(urlString);
                return true;
            }
            catch  
            {
                return false;
            }

            return false;
        }
    }
}
