using IdentityServer4.AuthServer.Models;
using IdentityServer4.AuthServer.Services;
using Microsoft.EntityFrameworkCore;
using static IdentityServer4.Models.IdentityResources;

namespace IdentityServer4.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _dbContext;
        public CustomUserRepository(CustomDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task<CustomUser> FindByEmail(string email)
        {
           var user =await _dbContext.CustomUsers.FirstOrDefaultAsync(x=>x.Email== email);
            if (user == null)
            {
                throw new Exception("User bulunamadı.");
            }
            return user;

        }

        public async Task<CustomUser> FindById(string userId)
        {
            var user = await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new Exception("User bulunamadı.");
            }
            return user;
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _dbContext.CustomUsers.AnyAsync(x => x.Email == email && x.Password==password);
          
        }
    }
}
