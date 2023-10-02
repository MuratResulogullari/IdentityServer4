using IdentityModel.Client;
using IdentityServer4.WebAppMvc1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace IdentityServer4.WebAppMvc1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var mail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            return View();
        }

        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");//Client çıkış yapıyor
            await HttpContext.SignOutAsync("oidc"); // Merkezi yerden çıkış yapıyor
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();
            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7287");
            if (disco == null)
            {
                throw new ArgumentNullException(nameof(disco));
            }
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));
            ClientConfiguration clientConfiguration = _configuration.GetSection("ClientConfiguration").Get<ClientConfiguration>();
            if (clientConfiguration == null)
                throw new ArgumentNullException(nameof(clientConfiguration));
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = clientConfiguration.ClientId,
                ClientSecret = clientConfiguration.ClientSecret,
                Address=disco.TokenEndpoint,
                RefreshToken = refreshToken,
            };
            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
                throw new Exception(nameof(token.Error));
            var tokens = new List<AuthenticationToken>() {
            new AuthenticationToken{Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken},
            new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
            new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
            new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            };
            var authenticationResult = await HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "administrator")]
        public IActionResult AdminAction()
        {
            return View();
        }
        [Authorize(Roles = "manager")]
        public IActionResult ManagerAction()
        {
            return View();
        }
    }
}