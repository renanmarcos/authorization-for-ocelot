using AuthorizationForOcelot.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MMLib.SwaggerForOcelot.DependencyInjection;

namespace ApiGatewayWithSwagger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                                .AddOcelotWithSwaggerSupport((options) =>
                                {
                                    options.Folder = "Configuration";
                                    options.HostEnvironment = hostingContext.HostingEnvironment;
                                })
                                .AddEnvironmentVariables()
                                .AddOcelotWithAuthorization(hostingContext.HostingEnvironment, options =>
                                {
                                    options.OcelotFilesFolder = "Configuration";
                                });
                        })
                        .UseStartup<Startup>()
                        .UseKestrel();
                });
    }
}
