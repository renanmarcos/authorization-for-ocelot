using AuthorizationForOcelot.SystemExtensions;
using Ocelot.Configuration.File;
using System.Collections.Generic;

namespace AuthorizationForOcelot.Configuration
{
    public class UserRolesRoute : FileRoute
    {
        private const string CatchAllPlaceHolder = "{everything}";

        public List<string> UserRoles { get; set; } = new List<string>();

        public bool CanCatchAll() => DownstreamPathTemplate.EndsWithIgnoringCase(CatchAllPlaceHolder);

        public bool HasMethodTransformation() => !string.IsNullOrWhiteSpace(DownstreamHttpMethod);
    }
}
