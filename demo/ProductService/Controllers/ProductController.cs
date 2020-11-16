using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> Get() => new List<Product>(3)
        {
            new Product()
            {
                Id = 2144,
                Name = "Smartphone",
                Price = 1199.99m
            },
            new Product()
            {
                Id = 23,
                Name = "Computer",
                Price = 949.99m
            }
        };

        [HttpPost]
        public dynamic Post() => new
        {
            Message = "Created product",
            Product = new Product()
            {
                Id = 22,
                Name = "Keyboard",
                Price = 29.99m
            }
        };
    }
}
