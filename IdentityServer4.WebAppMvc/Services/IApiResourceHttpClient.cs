using IdentityServer4.WebAppMvc1.Models;

namespace IdentityServer4.WebAppMvc1.Services
{
    public interface IApiResourceHttpClient
    {
        Task<HttpClient> GetHttpClientAsync();
        Task CreateUserAsync(UserInputModel model);
    }
}
