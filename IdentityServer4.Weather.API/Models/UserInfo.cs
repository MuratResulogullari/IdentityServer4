using Microsoft.AspNetCore.Identity;

namespace IdentityServer4.Weather.API.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
