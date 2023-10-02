namespace IdentityServer4.WebAppMvc2.Services
{
    public interface IApiResourceHttpClient
    {
        Task<HttpClient> GetHttpClientAsync();
    }
}
