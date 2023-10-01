using IdentityServer4.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = new List<Order>() {
            new Order(){ Name="Ali" ,Surname="Veli",Address="Konya",OrderItems =new List<OrderItem>{ new OrderItem() {PrductName="Kalem" ,Description="Dolma kalem",Price=50,Count=10,Image="Path" } } },
            new Order(){ Name="Ahmet" ,Surname="Hasanoğlu",Address="Erzurum",OrderItems =new List<OrderItem>{ new OrderItem() {PrductName="Defter" ,Description="Resim defteri",Price=75,Count=2,Image="Path" } } }
            };
            return Ok(orders);
        }
    }
}