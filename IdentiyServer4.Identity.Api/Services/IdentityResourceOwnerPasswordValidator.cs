using IdentityModel;
using IdentityServer4.Validation;
using IdentiyServer4.Identity.Api.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentiyServer4.Identity.Api.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager )
        {
            _userManager = userManager; 
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);
            if (existUser == null) return;
            var passwordCheck = await _userManager.CheckPasswordAsync(existUser,context.Password);
            if (!passwordCheck)
                return;
            context.Result = new GrantValidationResult(existUser.Id.ToString(),OidcConstants.AuthenticationMethods.Password);
        }
    }
}
