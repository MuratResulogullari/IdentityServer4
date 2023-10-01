using IdentityModel.Client;
using IdentityServer4.WebAppMvc1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityServer4.WebAppMvc1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<ProductViewModel>();
            HttpClient client = new HttpClient();
            ClientConfiguration clientConfiguration = _configuration.GetSection("ClientConfiguration").Get<ClientConfiguration>();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7287");
            if (disco.IsError)
            {
                throw new Exception(string.Join(" ", disco.Error));
            }
            var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                ClientId = clientConfiguration.ClientId,
                ClientSecret = clientConfiguration.ClientSecret,
                Address = disco.TokenEndpoint
            });
            if (token.IsError)
            {
                throw new Exception(string.Join(" ", token.Error));
            }
            client.SetBearerToken(token.AccessToken);

            var response = await client.GetAsync("https://localhost:7196/api/products/getproducts");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Response code: {response.StatusCode}  error.");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                 products = JsonConvert.DeserializeObject<List<ProductViewModel>>(content);
            }
            return View(products);
        }
    }
}