using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly CreationDate { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}
