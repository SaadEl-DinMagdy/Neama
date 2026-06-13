using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ReviewCreateDto
    {
        [Required] public int BranchId { get; set; }
        [Required] public string Comment { get; set; }
        [Required][Range(1, 5)] public int Rating { get; set; }
        [Required] public IFormFile Image { get; set; }
    }
}
