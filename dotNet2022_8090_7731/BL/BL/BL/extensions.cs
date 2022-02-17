using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BO;
using System.Device.Location;

namespace BL
{
    /// <summary>
    /// A public static class of extensions includes general functions .
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// A function that Calculates distance between all the places in the params,returns the distance in double.
        /// </summary>
        /// <param name="locations"></param>
        ///<returns>the function gets an array of locations and returns
        /// the sum of the distance between them
        /// in double.</returns>
        public static double CalculateDistance(params Location[] locations)
        {
            double distance = 0;
            for (int i = 0; i < locations.Length - 1; i++)
            {
                var location1 = geoCoordinate(locations[i]);
                var location2 = geoCoordinate(locations[i + 1]);
                distance += location1.GetDistanceTo(location2);
            }
            return distance;
        }

        /// <summary>
        /// A function that gets an instance of Location and bulids from it an instance of  
        /// GeoCoordinate and returns it.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>returns an instance of GeoCoordinate</returns>
        public static GeoCoordinate geoCoordinate(Location location)
        {
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }
    }
}
#region Erase?



//        static internal Dictionary<Type, Type> matchType = new()
//        {
//            [typeof(IDAL.DO.Drone)] = typeof(DroneToList),
//            [typeof(IDAL.DO.Customer)] = typeof(CustomerToList),
//            [typeof(IDAL.DO.Parcel)] = typeof(ParcelToList),
//            [typeof(IDAL.DO.BaseStation)] = typeof(StationToList),
//        };
//        //public static StringBuilder ToStringProps<T>(this T obj)
//        //{
//        //    return obj.ToStringProps();
//        //}
//        //public static void ToString<T>(this T obj)
//        //{
//        //    Console.WriteLine(Extensions.ToStringProps(obj));
//        //}
//        public static List<IDAL.DO.Drone> D = new();
//        public static List<IDAL.DO.Customer> C = new();
//        public static List<IDAL.DO.Parcel> P = new();
//        public static List<IDAL.DO.BaseStation> B = new();

//        public static List<DroneToList> Dr = new();
//        public static List<CustomerToList> Cu = new();
//        public static List<ParcelToList> Pa = new();
//        public static List<StationToList> St = new();

//        public static Dictionary<Type, IList> matchBL = new()
//        {
//            [typeof(Drone)] = D,
//            [typeof(Customer)] = C,
//            [typeof(Parcel)] = P,
//            [typeof(Station)] = B,
//        };

//        public static Dictionary<Type, IList> matchBLToList = new()
//        {
//            [typeof(Drone)] = Dr,
//            [typeof(Customer)] = Cu,
//            [typeof(Parcel)] = Pa,
//            [typeof(Station)] = St,
//        };
//        // public static Dictionary<Type, Type> matchBLToListObject = new()
//        //{
//        //    [typeof(IDAL.DO.Drone)] = typeof(DroneToList),
//        //    [typeof(IDAL.DO.Customer)] = typeof(CustomerToList),
//        //    [typeof(IDAL.DO.Parcel)] =typeof (ParcelToList),
//        //    [typeof(IDAL.DO.BaseStation)] = typeof(StationToList),
//        //};
#endregion