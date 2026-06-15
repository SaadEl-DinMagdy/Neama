using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public int BranchId { get; set; } 
        public DateTimeOffset CreationDate { get; set; }
    }
}
