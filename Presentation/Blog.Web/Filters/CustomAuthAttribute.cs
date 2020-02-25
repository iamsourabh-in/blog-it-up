using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Web.Filters
{
    public class CustomAuthFilterAttribute : ActionFilterAttribute, IAsyncAuthorizationFilter
    {
        public string ViewName { get; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/anonymus/Error?statusCode=401");
            }
        }

        private bool CheckUserPermission(ClaimsPrincipal user, string permission)
        {
            // Logic for checking the user permission goes here. 

            // Let's assume this user has only read permission.
            return permission == "Read";
        }
    }
}

