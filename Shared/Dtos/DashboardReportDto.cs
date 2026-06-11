using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class DashboardReportDto
    {
        public int TotalUsers { get; set; }
        public int TotalPartners { get; set; }
        public int TotalCharities { get; set; }
        public int TotalOfferedMeals { get; set; }
        public int TotalSavedMeals { get; set; } 
        public decimal TotalProfits { get; set; } 
    }
}
