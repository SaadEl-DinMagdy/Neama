using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class MainSection : BaseEntity
    {
        public string Name { get; set; }
        public string? IconURL { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Partner> Partners { get; set; } = new HashSet<Partner>();

        public DateOnly CreationDate { get; set; }
    }
}
