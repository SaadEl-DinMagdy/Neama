using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class BranchReviewsResponseDto
    {
        public RatingReportDto Report { get; set; }
        public IReadOnlyList<ReviewDto> Reviews { get; set; }
    }
}
