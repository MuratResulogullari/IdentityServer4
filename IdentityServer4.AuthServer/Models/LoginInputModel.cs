namespace IdentityServer4.AuthServer.Models
{
    public class LoginInputModel
    {

        public string ReturnUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
