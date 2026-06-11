using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class PartnerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal WalledBalance { get; set; }
        public bool Is_Active { get; set; }
        public string? Logo_URL { get; set; }
        public string? Cover_URL { get; set; }
        public DateOnly CreationDate { get; set; }

        public string? ManagerEmail { get; set; } 
        public string? MainSectionName { get; set; } 
    }
}
