using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        public DateOnly CreationDate { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public int? CategoryId { get; set; } 
        public Category? Category { get; set; }

    }
}
