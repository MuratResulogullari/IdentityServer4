using Microsoft.AspNetCore.Identity;
using System;

namespace IdentiyServer4.Identity.Api.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}