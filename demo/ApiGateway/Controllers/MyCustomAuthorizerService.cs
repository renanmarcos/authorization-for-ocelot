using AuthorizationForOcelot.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ApiGateway.Controllers
{
    public class MyCustomAuthorizerService : IAuthorizerForOcelot
    {
        public IEnumerable<string> FetchUserRoles(HttpRequest httpRequest) => 
            new List<string>()
            {
                "admin"
            };
    }
}
