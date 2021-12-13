using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace IBL.BO
{
    public static class Extensions
    {
        
        static internal Dictionary<Type, Type> matchType = new()
        {
            [typeof(IDal.DO.Drone)] = typeof(DroneToList),
            [typeof(IDal.DO.Customer)] = typeof(CustomerToList),
            [typeof(IDal.DO.Parcel)] = typeof(ParcelToList),
            [typeof(IDal.DO.BaseStation)] = typeof(StationToList),
        };
        //public static StringBuilder ToStringProps<T>(this T obj)
        //{
        //    return obj.ToStringProps();
        //}

        //public static void ToString<T>(this T obj)
        //{
        //    Console.WriteLine(Extensions.ToStringProps(obj));
        //}
        //public static Dictionary<Type, Type> matchBLObject = new()
        //{
        //    [typeof(IDal.DO.Drone)] = typeof(Drone),
        //    [typeof(IDal.DO.Customer)] = typeof(Customer),
        //    [typeof(IDal.DO.Parcel)] =typeof (Parcel),
        //    [typeof(IDal.DO.BaseStation)] = typeof(Station),
        //};

        // public static Dictionary<Type, Type> matchBLToListObject = new()
        //{
        //    [typeof(IDal.DO.Drone)] = typeof(DroneToList),
        //    [typeof(IDal.DO.Customer)] = typeof(CustomerToList),
        //    [typeof(IDal.DO.Parcel)] =typeof (ParcelToList),
        //    [typeof(IDal.DO.BaseStation)] = typeof(StationToList),
        //};
    }
}
