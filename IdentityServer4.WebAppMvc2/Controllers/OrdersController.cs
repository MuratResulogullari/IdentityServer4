using IdentityModel.Client;
using IdentityServer4.WebAppMvc2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace IdentityServer4.WebAppMvc2.Controllers
{
    [Authorize(Roles ="manager")]
    public class OrdersController : Controller
    {
        private readonly IConfiguration _configuration;

        public OrdersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var orders = new List<OrderViewModel>();
            HttpClient client = new HttpClient();
            ClientConfiguration clientConfiguration = _configuration.GetSection("ClientConfiguration").Get<ClientConfiguration>();
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            client.SetBearerToken(accessToken);
            var response = await client.GetAsync("https://localhost:7274/api/orders/getorders");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Response code: {response.StatusCode}  error.");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<OrderViewModel>>(content);
            }
            return View(orders);
        }
    }
}