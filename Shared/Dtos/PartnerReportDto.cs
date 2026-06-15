using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class PartnerReportDto
    {
        public int TotalBranches { get; set; }
        public int TotalSoldItems { get; set; }
        public decimal TotalSales { get; set; }
        public decimal NetProfit { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
