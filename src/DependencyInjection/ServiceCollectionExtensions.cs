using AuthorizationForOcelot.Abstractions;
using AuthorizationForOcelot.Configuration;
using AuthorizationForOcelot.DelegatingHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using System.Collections.Generic;

namespace AuthorizationForOcelot.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorizationWithOcelot<TAuth>(this IServiceCollection services, IConfiguration configuration) where TAuth : IAuthorizerForOcelot
        {
            services.Configure<List<UserRolesRoute>>(options => configuration.GetSection("Routes").Bind(options));
            services.AddSingleton(typeof(IAuthorizerForOcelot), typeof(TAuth));
            
            return services;
        }

        public static IOcelotBuilder AddAuthorizationOcelotHandler(this IOcelotBuilder builder)
        {
            builder.AddDelegatingHandler<AuthorizationHandler>(global: true);

            return builder;
        }
    }
}
