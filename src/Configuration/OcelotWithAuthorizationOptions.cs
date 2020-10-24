using Microsoft.Extensions.Hosting;

namespace AuthorizationForOcelot.Configuration
{
    public class OcelotWithAuthorizationOptions
    {
        public string OcelotConfigFileName { get; set; } = "ocelot.json";

        public IHostEnvironment HostEnvironment { get; set; }
    }
}
