using System.ComponentModel.DataAnnotations;

namespace Neama.Api.Dtos
{
    public class LoginDtos
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
