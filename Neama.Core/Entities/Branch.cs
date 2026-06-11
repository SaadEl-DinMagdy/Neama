using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Neama.Core.Entities
{
    public class Branch : BaseEntity
    {
        public string BranchName { get; set; }
        public string BranchPhone { get; set; }
        public bool Is_Active { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Point  Location { get; set; }    
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }

        public DateOnly CreationDate { get; set; }

        public string? ManagerId { get; set; }
        public AppUser? Manager { get; set; }

        public int? PartnerId { get; set; } 
        public Partner? Partner { get; set; }

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}
