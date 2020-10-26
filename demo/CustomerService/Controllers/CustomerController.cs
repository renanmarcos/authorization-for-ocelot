using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Customer> Get() => new List<Customer>(3)
        {
            new Customer()
            {
                Age = 20,
                Name = "John",
                Email = "john@mail.com"
            },
            new Customer()
            {
                Age = 23,
                Name = "Nick",
                Email = "nick@mail.com"
            },
            new Customer()
            {
                Age = 33,
                Name = "Ryan",
                Email = "ryan@mail.com"
            },
        };

        [HttpPost]
        public dynamic Post() => new
        {
            Message = "Created customer",
            Customer = new Customer()
            {
                Age = 22,
                Name = "Bob",
                Email = "bob@mail.com"
            }
        };
    }
}
