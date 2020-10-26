using AuthorizationForOcelot.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ApiGateway
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
                    webBuilder.UseKestrel()
                        .ConfigureAppConfiguration((hostingContext, config) => 
                        {
                            config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: false);
                            config.AddOcelotWithAuthorization(hostingContext.HostingEnvironment);                            
                        })
                        .UseStartup<Startup>();
                });
    }
}
