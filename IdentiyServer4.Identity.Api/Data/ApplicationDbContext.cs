using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentiyServer4.Identity.Api.Entities;
using System;
using Microsoft.AspNetCore.Identity;

namespace IdentiyServer4.Identity.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,string, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            builder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<ApplicationUserToken>().ToTable("UserTokens");
        }
    }
}
