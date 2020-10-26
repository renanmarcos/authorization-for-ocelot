using Microsoft.Extensions.Hosting;

namespace AuthorizationForOcelot.Configuration
{
    public class OcelotWithAuthorizationOptions
    {
        public string OcelotConfigFileName { get; set; } = "ocelot";

        public IHostEnvironment HostEnvironment { get; set; }
    }
}
