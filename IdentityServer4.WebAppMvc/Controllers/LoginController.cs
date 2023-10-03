using IdentityModel.Client;
using IdentityServer4.WebAppMvc1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace IdentityServer4.WebAppMvc1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerConfiguration:EndPoint"].ToString());
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            var password = new PasswordTokenRequest();
            password.ClientId = _configuration["ClientResourceOwnerConfiguration:ClientId"].ToString();
            password.ClientSecret = _configuration["ClientResourceOwnerConfiguration:ClientSecret"].ToString();
            password.Address = disco.TokenEndpoint;
            password.UserName = model.Email;
            password.Password = model.Password;
            var tokenResponse = await client.RequestPasswordTokenAsync(password);
            if (tokenResponse.IsError)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View();
                
            }

            var userInfoResponse = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Token = tokenResponse.AccessToken,
                Address = disco.UserInfoEndpoint
            });
            if (userInfoResponse.IsError)
            {
                ModelState.AddModelError("", "AccessToken hatalı");
                return View();
            }
            var claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims, CookieAuthenticationDefaults.AuthenticationScheme,"name","role");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.IdToken,Value=tokenResponse.IdentityToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.AccessToken,Value=tokenResponse.AccessToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.RefreshToken,Value=tokenResponse.RefreshToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)},
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");//Client çıkış yapıyor
           // await HttpContext.SignOutAsync("oidc"); // Merkezi yerden çıkış yapıyor
           return RedirectToAction("Index","Home");
        }
    }
}