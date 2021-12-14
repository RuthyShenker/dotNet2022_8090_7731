using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    ///A class of Location that contains:
    ///Longitude
    ///Latitude
    /// </summary>
    public class Location
    {
        /// <summary>
        /// A constructor of Location with fields.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public Location(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString() => this.ToStringProps();

    }
}
