using IdentityServer4.Weather.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer4.Weather.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccessor = httpContextAccesor;
        }
        public async Task<UserInfo> GetUserInfoAsync()
        {
            if (_httpContextAccessor.HttpContext != null)
            {

                var authResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

                if (authResult.Succeeded)
                {
                    if (authResult.Principal.Claims != null && authResult.Principal.Claims.Count() > 0)
                    {

                        if (authResult.Principal.Claims.Any(x => x.Type == "sub"))
                        {
                            var userId = authResult.Principal.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

                            var email = authResult.Principal.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

                            var phone = authResult.Principal.Claims.FirstOrDefault(x => x.Type == "phone_number")?.Value;

                            return new UserInfo
                            {
                                UserId = userId,
                                Email = email,
                                PhoneNumber = phone,

                            };
                        }
                    }
                }

            }

            return new();
        }
    }
}
