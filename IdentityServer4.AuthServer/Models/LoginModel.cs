namespace IdentityServer4.AuthServer.Models
{
    public class LoginModel
    {
        public string? ReturnUrl { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
