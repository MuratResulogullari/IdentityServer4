using System.ComponentModel.DataAnnotations;

namespace IdentiyServer4.Identity.Api.Models
{
    public class UserInputModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

