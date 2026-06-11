using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.shareEnumsAndEntitys
{
    public class ShippingAddress
    {
        public ShippingAddress() { }

        public ShippingAddress(string name, string phoneNumber,double longitude, double latitude, string distinctiveMark, string street, string city, string governorate, string displayName )
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
            DistinctiveMark = distinctiveMark;
            Street = street;
            City = city;
            Governorate = governorate;
            DisplayName = displayName;
            PhoneNumber = phoneNumber;
        }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string DistinctiveMark { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string DisplayName { get; set; }
    }
}
