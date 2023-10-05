using Microsoft.AspNetCore.Identity;

namespace IdentiyServer4.Identity.Api.Entities
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
       
        public string ServiceId { get; set; }
        public string ControllerId { get; set; }
    }
}