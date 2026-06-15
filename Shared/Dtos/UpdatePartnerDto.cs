using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UpdatePartnerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int mainSectionId { get; set; }
        public IFormFile? Logo { get; set; }
        public IFormFile? Cover { get; set; }
    }
}
