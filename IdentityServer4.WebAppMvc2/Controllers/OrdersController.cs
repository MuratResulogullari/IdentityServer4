using IdentityModel.Client;
using IdentityServer4.WebAppMvc2.Models;
using IdentityServer4.WebAppMvc2.Services;
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
        private readonly IApiResourceHttpClient _apiResourceHttpClient;
        public OrdersController(IConfiguration configuration
            ,IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var orders = new List<OrderViewModel>();
            ClientConfiguration clientConfiguration = _configuration.GetSection("ClientConfiguration").Get<ClientConfiguration>();
            
            var client = await _apiResourceHttpClient.GetHttpClientAsync();
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