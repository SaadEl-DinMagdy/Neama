using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public int MealsSaved { get; set; }
        public decimal SavedMoney { get; set; }

        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public Partner? Partner { get; set; }
        public Charity? Charity { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<UserProduct> UserProduct { get; set; } = new HashSet<UserProduct>();
    }
}
