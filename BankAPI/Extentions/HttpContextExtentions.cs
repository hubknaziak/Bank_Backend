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
        public static string GetEmailFromClaims(this HttpContext httpContext)
        {
            var emailClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
            if (emailClaim == null)
            {
                return null;
            }
            return emailClaim.Value;
        }
    }
}
