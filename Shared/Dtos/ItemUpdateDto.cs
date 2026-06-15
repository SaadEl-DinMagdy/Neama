using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ItemUpdateDto
    {
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        [Required] public decimal OriginalPrice { get; set; }
        [Required] public decimal DiscountPrice { get; set; }
        [Required] public int StockQuantity { get; set; }
        public DateOnly? ExpiryDate { get; set; } 
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
