using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; }
        public int BranchId { get; set; }

        public AppUser User { get; set; }
        public Branch Branch { get; set; }
    }
}
