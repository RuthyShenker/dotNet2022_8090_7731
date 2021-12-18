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
        //public override string ToString()
        //{
        //    double latitude = Latitude;
        //    double longitude = Longitude;
        //    string lat()
        //    {
        //        string ch = "N";
        //        if (latitude < 0)
        //        {
        //            ch = "S";
        //            latitude = -latitude;
        //        }
        //        int deg = (int)latitude;
        //        int min = (int)(60 * (latitude - deg));
        //        double sec = (latitude - deg) * 3600 - min * 60;
        //        return $"{deg}° {min}′ {sec}″ {ch}";
        //    }

        //    string lng()
        //    {
        //        string ch = "E";
        //        if (longitude < 0)
        //        {
        //            ch = "W";
        //            longitude = -longitude;
        //        }

        //        int deg = (int)longitude;
        //        int min = (int)(60 * (longitude - deg));
        //        double sec = (longitude - deg) * 3600 - min * 60;
        //        return $"{deg}° {min}′ {sec}″ {ch}";
        //    }

        //    return $"{lat()} {lng()}";
        //}

    }
}
