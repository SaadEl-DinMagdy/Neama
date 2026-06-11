using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AddUserProduct
    {
        [Required]
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Discription { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [Phone]
        [MaxLength(15)]
        public string Phone { get; set; }
        [Required]
        [Phone]
        [MaxLength(15)]
        public string WhatsApp { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
