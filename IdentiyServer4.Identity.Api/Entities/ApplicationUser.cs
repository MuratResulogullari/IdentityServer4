using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentiyServer4.Identity.Api.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName, string name, string surname) : base(userName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.Surname = surname;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Title { get; set; }
        public int DefaultCultere { get; set; }
        public bool Enabled { get; set; } = true!;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<ApplicationUserClaim> UserClaims { get; set; }
        public virtual ICollection<ApplicationUserLogin> UserLogins { get; set; }
        public virtual ICollection<ApplicationUserToken> UserTokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}