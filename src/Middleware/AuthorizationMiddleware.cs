using AuthorizationForOcelot.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationForOcelot.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<List<UserRolesRoute>> _routes;

        public AuthorizationMiddleware(RequestDelegate next, IOptions<List<UserRolesRoute>> routes)
        {
            _next = next;
            _routes = routes;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }
}
