namespace IdentityServer4.AuthServer.Models
{
    public class CustomUser
    {
        public CustomUser()
        {
            this.Id=Guid.NewGuid().ToString();
        }
        public string Id { get; init; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
    }
}
