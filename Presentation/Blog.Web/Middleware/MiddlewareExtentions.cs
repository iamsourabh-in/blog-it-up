using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Middleware
{
    public static class MiddlewareExtentions
    {
        public static IApplicationBuilder UseAPIRequestMorpher(
           this IApplicationBuilder builder, bool active)
        {
            return builder.UseMiddleware<APIRequestMorpher>();
        }
    }
}
