using System;

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
