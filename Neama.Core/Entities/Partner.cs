using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class Partner : BaseEntity
    {
        public string Name { get; set; }

        public decimal WalledBalance { get; set; }
        public bool Is_Active { get; set; }
        public string? Logo_URL { get; set; }
        public string? Cover_URL { get; set; }

        public DateOnly CreationDate { get; set; }

        public string? ManagerId { get; set; }
        public AppUser? Manager { get; set; }

        public int? MainSectionId { get; set; } 
        public MainSection? MainSection { get; set; } 


        public ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();

    }
}
