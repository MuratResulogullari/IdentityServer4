using IdentityModel;
using IdentityModel.Client;
using IdentityServerHost.Quickstart.UI;
using IdentiyServer4.Identity.Api.Entities;
using IdentiyServer4.Identity.Api.Models;
using IdentiyServer4.Identity.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using static IdentityServer4.IdentityServerConstants;

namespace IdentiyServer4.Identity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   // [Authorize(LocalApi.PolicyName)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser>   _userManager;   
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsPrincipalFactory;
        private readonly ApplicationUserManager _applicationUserManager;
        public UsersController(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            , RoleManager<ApplicationRole> roleManager
            , IHttpContextAccessor httpContextAccessor
            , ApplicationUserManager applicationUserManager,
IUserClaimsPrincipalFactory<ApplicationUser> claimsPrincipalFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _applicationUserManager = applicationUserManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;   
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody]UserInputModel model)
        {
            var user = new ApplicationUser(model.UserName, model.Name, model.Surname)
            {
                Email= model.Email,
                CreatedBy = "administrator",
                CreatedOn=DateTime.UtcNow,

            };
            var result=await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x=>x.Description));
            }
            return Ok("User created successful.");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model )
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("Username not found");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,true );
            if (!result.Succeeded)
            {
                return BadRequest("password error");
            }
            await _signInManager.SignInAsync(user, model.RememberLogin);
            return Ok($"Başarılı");
        }
        [HttpGet("getUserProfile")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserProfile()
        {

            var authResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);


            if (authResult.Succeeded)
            {

                var userId = authResult.Principal.Claims.First(x => x.Type == JwtClaimTypes.Subject).Value;


                var user = await _applicationUserManager.FindByIdAsync(userId);
                var principle = await _claimsPrincipalFactory.CreateAsync(user);
                var userClaims =_applicationUserManager.GetUserProfileClaims(principle);

                userClaims.Add(new ClaimDto
                {
                    Type = IdentityModel.JwtClaimTypes.Subject,
                    Value = user.Id
                });

                userClaims.Add(new ClaimDto
                {
                    Type = IdentityModel.JwtClaimTypes.PreferredUserName,
                    Value = user.UserName
                });

                userClaims.Add(new ClaimDto
                {
                    Type = IdentityModel.JwtClaimTypes.Email,
                    Value = user.Email
                });

                userClaims.Add(new ClaimDto
                {
                    Type = IdentityModel.JwtClaimTypes.PhoneNumber,
                    Value = user.PhoneNumber
                });

                userClaims.Add(new ClaimDto
                {
                    Type = IdentityModel.JwtClaimTypes.Role,
                    Value = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                });

                return Ok(userClaims);
            }


            return Ok(null);

        }
    }

}