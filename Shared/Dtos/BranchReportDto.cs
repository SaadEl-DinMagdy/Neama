using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class BranchReportDto
    {
        public decimal TotalSales { get; set; }
        public decimal TotalProfits { get; set; }
        public int DisplayedItemsCount { get; set; }
        public int CompletedOrdersCount { get; set; }
    }
}
