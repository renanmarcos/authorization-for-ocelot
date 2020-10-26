using System.Collections.Generic;

namespace AuthorizationForOcelot.Configuration
{
    public class AuthorizationFileConfiguration
    {
        public List<UserRolesRoute> Routes { get; set; } = new List<UserRolesRoute>();
    }
}
