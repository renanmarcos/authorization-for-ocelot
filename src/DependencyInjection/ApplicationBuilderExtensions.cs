using AuthorizationForOcelot.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AuthorizationForOcelot.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOcelotWithAuthorization(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationMiddleware>();

            return app;
        }
    }
}
