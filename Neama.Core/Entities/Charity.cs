using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class Charity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public string Phone { get; set; }
        public bool Is_Active { get; set; }
        public DateOnly CreationDate { get; set; }

        public string? ManagerId { get; set; }
        public AppUser? Manager { get; set; }
    }
}
