using IdentityServer4.AuthServer.Models;

namespace IdentityServer4.AuthServer.Services
{
    public interface ICustomUserRepository
    {
        Task<bool> Validate(string email,string password);
        Task<CustomUser> FindById(string userId);
        Task<CustomUser> FindByEmail(string email);
    }
}