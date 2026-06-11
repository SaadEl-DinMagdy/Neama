using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class NominatimResponse
    {

        public string display_name { get; set; }


        public AddressDetails address { get; set; }
    }
}
