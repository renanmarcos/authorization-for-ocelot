using AuthorizationForOcelot.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationForOcelot.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<UserRolesRoute> _routes;

        public AuthorizationMiddleware(RequestDelegate next, IOptions<List<UserRolesRoute>> routes)
        {
            _next = next;
            _routes = routes.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsOptionsHttpMethod(context) && FindRoute(context.Request) is UserRolesRoute configuredRoute)
            {
                bool canAccess = GetCurrentRolesFromIDM(context)
                    .Any(userRole =>
                        configuredRoute.UserRoles.Any(allowedRouteRole => allowedRouteRole.Equals(userRole)));

                if (canAccess) 
                    await _next(context);

                JsonResult result = new JsonResult(new
                {
                    Message = "Not Authorized."
                });
                ActionContext actionContext = new ActionContext(context, context.GetRouteData(), new ActionDescriptor());
                await result.ExecuteResultAsync(actionContext);
            } 
            
            await _next(context);
        }

        private UserRolesRoute FindRoute(HttpRequest request)
        {
            return _routes.FirstOrDefault(route =>
            {
                bool isSamePath = true;
                List<string> downstreamHosts = route.DownstreamHostAndPorts.Select(h => h.Host).ToList();
                
                if (!route.CanCatchAll())
                {
                    isSamePath = request.Path.Value.Contains(route.DownstreamPathTemplate, StringComparison.CurrentCultureIgnoreCase);
                }

                return isSamePath &&
                       downstreamHosts.Any(d => d.Equals(request.Host.Host, StringComparison.CurrentCultureIgnoreCase)) &&
                       route.UpstreamHttpMethod.Any(u => u.Equals(request.Method, StringComparison.CurrentCultureIgnoreCase)) &&
                       request.Scheme.Equals(route.DownstreamScheme, StringComparison.CurrentCultureIgnoreCase);
            });
        }

        private bool IsOptionsHttpMethod(HttpContext httpContext)
        {
            return httpContext.Request.Method.Equals("Options", StringComparison.CurrentCultureIgnoreCase);
        }

        private List<string> GetCurrentRolesFromIDM(HttpContext context)
        {
            // Get access token and get scopes from IDM here

            return new List<string>()
            {
                "user"
            };
        }
    }
}
