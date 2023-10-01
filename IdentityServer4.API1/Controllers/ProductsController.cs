using IdentityServer4.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Authorize]
        // /api/products
        [HttpGet]
        [Authorize(policy: "ReadProduct")]
        public IActionResult GetProducts()
        {
            var products = new List<Product>()
                {
                    new Product (){Name="Product 1",Price=12324,Stock=10},
                    new Product (){Name="Product 2",Price=124,Stock=23234},
                    new Product (){Name="Product 3",Price=324,Stock=35334},
                    new Product (){Name="Product 4",Price=4,Stock=5675},
                };

            return Ok(products);
        }
        [Authorize(policy: "CreateOrUpdateProduct")]
        [HttpPost]
        public IActionResult UpdateProduct([FromQuery] string productId,[FromQuery] string name)
        {

            return Ok($"{productId} id li ürün ismi {name} güncellendi.");
        }
        [Authorize(policy: "CreateOrUpdateProduct")]
        [HttpPost]
        public IActionResult CreateProduct([FromForm] Product product)
        {
            return Ok($"{product.Name} isminde yeni ürün başaşrıyla eklendi");
        }
    }
}