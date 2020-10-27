using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayWithSwagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloSwaggerController : ControllerBase
    {
        [HttpGet]
        public dynamic Get() => new { Message = "Running an API Gateway with Swagger." };
    }
}
