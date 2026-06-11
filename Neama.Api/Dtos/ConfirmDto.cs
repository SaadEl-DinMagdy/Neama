using System.ComponentModel.DataAnnotations;

namespace Neama.Api.Dtos
{
    public class ConfirmDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
