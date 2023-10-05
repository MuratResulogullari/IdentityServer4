using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentiyServer4.Identity.Api.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole(string? parentRoleId, string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            ParentRoleId = string.IsNullOrEmpty(parentRoleId) ? null : parentRoleId;
            this.Name = name;
            this.Description = description;
        }

        public string Description { get; set; }
        public ushort LanguageId { get; set; }
        public bool Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? ParentRoleId { get; set; }
        public virtual ApplicationRole ParentRole { get; set; }
        public virtual ICollection<ApplicationRole> SubRoles { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}