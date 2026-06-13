using System.ComponentModel.DataAnnotations;

namespace Neama.Api.Dtos
{
    public class ResendEmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
