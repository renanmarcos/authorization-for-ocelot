using Ocelot.Configuration.File;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AuthorizationForOcelot.Configuration
{
    public class UserRolesRoute : FileRoute
    {
        private const string CatchAllPlaceHolderPattern = "({[^}]*})";
        private const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        public List<string> UserRoles { get; set; } = new List<string>();

        public bool CanCatchAll() => Regex.IsMatch(DownstreamPathTemplate, CatchAllPlaceHolderPattern, Options) && 
                                     Regex.IsMatch(UpstreamPathTemplate, CatchAllPlaceHolderPattern, Options);

        public bool HasMethodTransformation() => !string.IsNullOrWhiteSpace(DownstreamHttpMethod);
    }
}
