using System.Text.Json;
using AuthorizationForOcelot.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGatewayWithSwagger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerForOcelot(Configuration);
            services.AddSwaggerGen(setup =>
            {
                OpenApiInfo apiInfo = new OpenApiInfo
                {
                    Title = "API Gateway with Swagger",
                    Version = "v1"
                };

                setup.SwaggerDoc("v1", apiInfo);
                setup.DocumentFilter<HideOcelotControllersFilter>();
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddAuthorizationWithOcelot(Configuration);
            services.AddOcelot();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseSwagger();
            app.UseSwaggerForOcelotUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.DocumentTitle = "API Gateway";
            });

            app.UseOcelotWithAuthorization();

            app.UseOcelot().Wait();
        }
    }
}
