using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UserProductDto
    {
        public List<IFormFile>? Photos { get; set; } = new List<IFormFile>();
        [Required]
        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]

        public string Discription { get; set; }
        [Required]

        public string Country { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Phone { get; set; }
        [Required]

        public string WhatsApp { get; set; }
        [Required]
        public double Price { get; set; }
        public DateOnly CreationDate { get; set; }
    }
}
