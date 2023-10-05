using IdentityModel.Client;
using IdentityServer4.WebAppMvc2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer4.WebAppMvc2.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor
            , HttpClient httpClient
            , IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.SetBearerToken(accessToken);
            }
            return _httpClient;
        }

    }
}