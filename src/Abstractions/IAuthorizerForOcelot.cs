using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AuthorizationForOcelot.Abstractions
{
    public interface IAuthorizerForOcelot
    {
        IEnumerable<string> FetchUserRoles(HttpRequest httpRequest);
    }
}
