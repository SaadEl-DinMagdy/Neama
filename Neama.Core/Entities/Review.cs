using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class Review : BaseEntity
    {
        public string Comment { get; set; }
        public int Rating { get; set; } 
        public string ImageUrl { get; set; }
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
