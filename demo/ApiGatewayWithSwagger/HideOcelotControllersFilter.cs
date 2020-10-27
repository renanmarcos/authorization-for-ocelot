using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiGatewayWithSwagger
{
    public class HideOcelotControllersFilter : IDocumentFilter
    {
        private static readonly string[] _ignoredPaths = {
            "/configuration",
            "/outputcache/{region}"
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (string ignorePath in _ignoredPaths)
            {
                swaggerDoc.Paths.Remove(ignorePath);
            }
        }
    }
}
