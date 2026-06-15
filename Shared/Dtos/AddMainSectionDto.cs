using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AddMainSectionDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile IconURL { get; set; }
    }
}
