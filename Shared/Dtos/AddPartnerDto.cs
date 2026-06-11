using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AddPartnerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? ManagerId { get; set; }
    }
}
