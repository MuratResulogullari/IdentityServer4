using IdentityServer4.Weather.API.Models;

namespace IdentityServer4.Weather.API.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync();
    }
}
