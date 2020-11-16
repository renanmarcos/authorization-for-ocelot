namespace AuthorizationForOcelot.Configuration
{
    public class OcelotWithAuthorizationOptions
    {
        public string OcelotConfigFileName { get; set; } = "ocelot";

        public string OcelotFilesFolder { get; set; } = string.Empty;
    }
}
