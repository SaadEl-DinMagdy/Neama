using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class BranchDetailsDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string BranchPhone { get; set; }
        public bool Is_Active { get; set; }
        public string? Email { get; set; }
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int SoldItems { get; set; }
        public decimal BranchSales { get; set; }
        public decimal BranchNetProfit { get; set; }
    }
}
