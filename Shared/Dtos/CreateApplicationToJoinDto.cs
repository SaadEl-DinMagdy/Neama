using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class CreateApplicationToJoinDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string PlaceName { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
    }
}
