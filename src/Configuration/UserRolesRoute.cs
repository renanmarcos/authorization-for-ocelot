using Ocelot.Configuration.File;
using System.Collections.Generic;

namespace AuthorizationForOcelot.Configuration
{
    public class UserRolesRoute : FileRoute
    {
        public List<string> UserRoles { get; set; } = new List<string>();
    }
}
