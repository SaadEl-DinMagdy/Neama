using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AllItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Discount { get; set; }
        public int StockQuantity { get; set; }
        public DateOnly? ExpiryDate { get; set; }
    }
}
