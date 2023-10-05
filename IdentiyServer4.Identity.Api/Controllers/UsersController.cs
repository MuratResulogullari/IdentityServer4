using IdentityModel.Client;
using IdentityServerHost.Quickstart.UI;
using IdentiyServer4.Identity.Api.Entities;
using IdentiyServer4.Identity.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static IdentityServer4.IdentityServerConstants;

namespace IdentiyServer4.Identity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser>   _userManager;   
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
    }
}