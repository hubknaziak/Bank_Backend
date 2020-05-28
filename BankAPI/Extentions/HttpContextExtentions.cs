using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BankAPI.Extentions
{
    public static class HttpContextExtentions
    {
        public static string GetLoginFromClaims(this HttpContext httpContext)
        {
            var loginClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
            if (loginClaim == null)
            {
                return null;
            }
            return loginClaim.Value;
        }
    }
}
