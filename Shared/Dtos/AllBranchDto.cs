using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AllBranchDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public decimal AverageRating { get; set; }
        public double distance { get; set; }
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public int ItemavilableCount { get; set; }

        public string? Logo_URL { get; set; }
        public string? Cover_URL { get; set; }

    }
}
