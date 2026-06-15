using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class ApplicationsToJoin : BaseEntity
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string PlaceName { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public bool ContactWasMade { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
