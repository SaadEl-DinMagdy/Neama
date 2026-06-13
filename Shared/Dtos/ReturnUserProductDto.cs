using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ReturnUserProductDto
    {
        public List<string> Photos { get; set; } = new List<string>();
        public int Id { get; set; }

        public string Name { get; set; }

        public string Discription { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string WhatsApp { get; set; }

        public double Price { get; set; }
        public DateOnly CreationDate { get; set; }
    }
}
