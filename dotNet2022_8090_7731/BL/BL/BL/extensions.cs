//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BL;

//namespace BL
//{
//    public static class Extensions
//    {

//        static internal Dictionary<Type, Type> matchType = new()
//        {
//            [typeof(IDal.DO.Drone)] = typeof(DroneToList),
//            [typeof(IDal.DO.Customer)] = typeof(CustomerToList),
//            [typeof(IDal.DO.Parcel)] = typeof(ParcelToList),
//            [typeof(IDal.DO.BaseStation)] = typeof(StationToList),
//        };
//        //public static StringBuilder ToStringProps<T>(this T obj)
//        //{
//        //    return obj.ToStringProps();
//        //}

//        //public static void ToString<T>(this T obj)
//        //{
//        //    Console.WriteLine(Extensions.ToStringProps(obj));
//        //}
//        public static List<IDal.DO.Drone> D = new();
//        public static List<IDal.DO.Customer> C = new();
//        public static List<IDal.DO.Parcel> P = new();
//        public static List<IDal.DO.BaseStation> B = new();

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
//        //    [typeof(IDal.DO.Drone)] = typeof(DroneToList),
//        //    [typeof(IDal.DO.Customer)] = typeof(CustomerToList),
//        //    [typeof(IDal.DO.Parcel)] =typeof (ParcelToList),
//        //    [typeof(IDal.DO.BaseStation)] = typeof(StationToList),
//        //};
//    }
//}
