using AuthorizationForOcelot.Configuration;
using AuthorizationForOcelot.SystemExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationForOcelot.DelegatingHandlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly List<UserRolesRoute> _routes;
        private readonly IHttpContextAccessor _httpUpstreamContextAccessor;

        public AuthorizationHandler(IOptions<List<UserRolesRoute>> routes, IHttpContextAccessor httpUpstreamContextAccessor)
        {
            _routes = routes.Value;
            _httpUpstreamContextAccessor = httpUpstreamContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage downstreamRequest, CancellationToken cancellationToken)
        {
            if (!IsOptionsHttpMethod(downstreamRequest) && FindRoute(downstreamRequest) is UserRolesRoute configuredRoute)
            {
                if (UserCanAccessResource(downstreamRequest, configuredRoute))
                {
                    return await base.SendAsync(downstreamRequest, cancellationToken);
                }

                dynamic responseObject = new
                {
                    Message = "User cannot access this resource."
                };
                JsonResult result = new JsonResult(responseObject)
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
                ActionContext actionContext = new ActionContext(_httpUpstreamContextAccessor.HttpContext, _httpUpstreamContextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());

                await result.ExecuteResultAsync(actionContext);
            }

            return await base.SendAsync(downstreamRequest, cancellationToken);
        }

        private bool IsOptionsHttpMethod(HttpRequestMessage downstreamRequest)
        {
            return downstreamRequest.Method.Method.EqualsIgnoringCase("options");
        }

        private UserRolesRoute FindRoute(HttpRequestMessage downstreamRequest)
        {
            return _routes.FirstOrDefault(route =>
            {
                bool isSameDownstreamPath = true, isSameDownstreamHttpMethod;

                if (!route.CanCatchAll())
                {
                    isSameDownstreamPath = downstreamRequest.RequestUri.LocalPath.ContainsIgnoringCase(route.DownstreamPathTemplate);
                }

                if (route.HasMethodTransformation())
                {
                    isSameDownstreamHttpMethod = route.DownstreamHttpMethod.EqualsIgnoringCase(downstreamRequest.Method.Method);
                }
                else
                {
                    isSameDownstreamHttpMethod = route.UpstreamHttpMethod.Any(u => u.EqualsIgnoringCase(downstreamRequest.Method.Method));
                }

                return isSameDownstreamPath &&
                       route.DownstreamHostAndPorts.Any(d => d.Host.EqualsIgnoringCase(downstreamRequest.RequestUri.Host) && d.Port.Equals(downstreamRequest.RequestUri.Port)) &&
                       isSameDownstreamHttpMethod &&
                       route.DownstreamScheme.EqualsIgnoringCase(downstreamRequest.RequestUri.Scheme);
            });
        }

        private bool UserCanAccessResource(HttpRequestMessage downstreamRequest, UserRolesRoute configuredUserRolesForRoute)
        {
            if (configuredUserRolesForRoute.UserRoles == null || configuredUserRolesForRoute.UserRoles.Count == 0)
                return true;

            return GetCurrentRolesFromIDM(downstreamRequest)
                    .Any(userRole => 
                        configuredUserRolesForRoute.UserRoles
                            .Any(allowedRouteRole => allowedRouteRole.EqualsIgnoringCase(userRole)));

        }

        private List<string> GetCurrentRolesFromIDM(HttpRequestMessage downstreamRequest)
        {
            // Get access token and get scopes from IDM here
            return new List<string>()
            {
                "user"
            };
        }
    }
}
