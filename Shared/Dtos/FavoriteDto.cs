using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BranchId { get; set; }
    }
}
