using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace workToolApi.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.Filters.Any(item => item is IAllowAnonymousFilter)) {
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated) {
                context.Result = new ValidErrorResult("验证失败");

            }

            //if(context.u)
        }
    }



    public class ValidErrorResult : ObjectResult
    {
        public ValidErrorResult(object value) : base(value)
        {
            StatusCode = (int)HttpStatusCode.OK;
        }
    }

}
