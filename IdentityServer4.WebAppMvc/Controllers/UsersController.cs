using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer4.WebAppMvc1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            var user = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier);
            var userId = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier).Value;
            var mail = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Email);
            return View();
        }
    }
}
