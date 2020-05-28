using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BankCore.Utility;
using BankAPI.Extentions;

namespace BankAPI.Filters
{
    public class ValidateTokenFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAnonymousAllowed = context.ActionDescriptor.EndpointMetadata
                    .Any(x => x.GetType() == typeof(AllowAnonymousAttribute));

            if (isAnonymousAllowed)
            {
                return;
            }

            var userLogin = context.HttpContext.GetLoginFromClaims();
            if (userLogin == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Error: HttpContext.User.LoginClaim is null");
                return;
            }

            var isValidated = TokenUtility.Validate(userLogin,
                context.HttpContext.Request.Headers);

            if (!isValidated)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Error: Token is not valid!");
                return;
            }
        }
    }
}
