﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Web.Extensions
{
    public static class IsAjaxRequestExt
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return !string.IsNullOrEmpty(request.Headers["X-Requested-With"]) &&
                    string.Equals(
                        request.Headers["X-Requested-With"],
                        "XmlHttpRequest",
                        StringComparison.OrdinalIgnoreCase);

            return false;
        }
    }
}
