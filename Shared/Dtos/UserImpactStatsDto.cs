using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UserImpactStatsDto
    {
        public int TotalSavedMeals { get; set; }
        public decimal TotalSavedAmount { get; set; }
    }
}
