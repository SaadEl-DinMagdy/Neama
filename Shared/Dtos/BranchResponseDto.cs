using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class BranchResponseDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string BranchPhone { get; set; }
        public bool Is_Active { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public DateOnly CreationDate { get; set; }
    }
}
