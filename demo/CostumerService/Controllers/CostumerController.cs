using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CostumerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CostumerController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Costumer> Get() => new List<Costumer>(3)
        {
            new Costumer()
            {
                Age = 20,
                Name = "John",
                Email = "john@mail.com"
            },
            new Costumer()
            {
                Age = 23,
                Name = "Nick",
                Email = "nick@mail.com"
            },
            new Costumer()
            {
                Age = 33,
                Name = "Ryan",
                Email = "ryan@mail.com"
            },
        };
    }
}
