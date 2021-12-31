using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Location(double latitude,double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public Location() { }
    }
}
