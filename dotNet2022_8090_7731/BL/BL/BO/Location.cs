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
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        /// <summary>
        /// A function that returns the location.
        /// </summary>
        /// <returns>returns the location.</returns>
        public override string ToString() => this.ToStringProps();
    }
}

