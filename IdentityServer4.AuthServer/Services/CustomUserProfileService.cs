using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer4.AuthServer.Services
{
    public class CustomUserProfileService : IProfileService
    {
        private readonly ICustomUserRepository _customUserRepository;

        public CustomUserProfileService(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _customUserRepository.FindById(userId);
            var claims = new List<Claim>()
            {
                new Claim("name",user.Username),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("city",user.City),
            };
            if (user.Username == "murat.resulogullari")
                claims.Add(new Claim("role", "administrator"));
            if (user.City == "Konya")
                claims.Add(new Claim("role", "manager"));

           // context.AddRequestedClaims(claims); // Doğru olanı bu dur
            context.IssuedClaims = claims;// amam buda kullanılır token içerisine gömer claimsleri
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _customUserRepository.FindById(userId);
            context.IsActive = user==null?false:true;
        }
    }
}