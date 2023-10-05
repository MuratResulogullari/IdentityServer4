using IdentityModel.Client;
using IdentityServer4.WebAppMvc1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text;

namespace IdentityServer4.WebAppMvc1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private  HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ApiResourceHttpClient(
            IHttpContextAccessor httpContextAccessor
            , HttpClient httpClient
            , IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = await _httpContextAccessor.HttpContext?.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.SetBearerToken(accessToken);
            }
            //var disco =await _httpClient.GetDiscoveryDocumentAsync(_configuration["IdentityApiConfiguration:EndPoint"]);
            //if (disco.IsError) {
            // throw new Exception(disco.Error);
            //}
            //var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();
            //clientCredentialsTokenRequest.ClientId = _configuration["ClientResourceOwnerConfiguration:ClientId"];
            //clientCredentialsTokenRequest.ClientSecret = _configuration["ClientResourceOwnerConfiguration:ClientSecret"];
            //clientCredentialsTokenRequest.Address = disco.TokenEndpoint;

            //var tokenResponse=await _httpClient.RequestTokenAsync(clientCredentialsTokenRequest);
            //if (tokenResponse.IsError)
            //    throw new Exception(tokenResponse.Error);

            //_httpClient.SetBearerToken(tokenResponse.AccessToken);

            return _httpClient;
        }

        public async Task CreateUserAsync(UserInputModel model)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(_configuration["IdentityApiConfiguration:EndPoint"]);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();
            clientCredentialsTokenRequest.ClientId = _configuration["ClientResourceOwnerConfiguration:ClientId"];
            clientCredentialsTokenRequest.ClientSecret = _configuration["ClientResourceOwnerConfiguration:ClientSecret"];
            clientCredentialsTokenRequest.Address = disco.TokenEndpoint;
            clientCredentialsTokenRequest.GrantType=OpenIdConnectGrantTypes.ClientCredentials;
            var tokenResponse = await _httpClient.RequestTokenAsync(clientCredentialsTokenRequest);
            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error);

            _httpClient.SetBearerToken(tokenResponse.AccessToken);

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse =await _httpClient.PostAsync("https://localhost:5022/api/users/createuser", content);
            if (!httpResponse.IsSuccessStatusCode)
            {
                var error=JsonConvert.DeserializeObject<List<string>>(await httpResponse.Content.ReadAsStringAsync());
                throw new Exception(string.Join(" ",error.Select(x=>x.ToString()).ToList()));
            }

        }
    }
}