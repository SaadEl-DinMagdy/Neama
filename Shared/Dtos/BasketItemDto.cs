using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        [Range(0.1, double.MaxValue, ErrorMessage = "Price Must be Greater than Zero!!")]
        public decimal Price { get; set; }
        public int BranchId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must be at least ONE!!")]
        public int Quantity { get; set; }
    }
}
