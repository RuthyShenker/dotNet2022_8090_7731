using DalObject;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using System.Collections;

namespace IDal.DO
{
    public static class Extensions
    {

        public static IIdentifiable GetFromDalById<T>(int Id) where T : IIdentifiable
        {
            return (IIdentifiable)DataSource.data[typeof(T)].Cast<IIdentifiable>().Where(item => item.Id == Id);
        }


        //public static IEnumerable GetListFromDal<T>() where T : IIdentifiable
        //{
        //    return DataSource.data[typeof(T)];
        //}
        public static IEnumerable<T> GetListFromDal<T>() where T : IIdentifiable
        {
            return (IEnumerable < T > )DataSource.data[typeof(T)];
        }

        //static IEnumerable GetListFromDal(Type type)
        //{
        //    return type switch
        //    {
        //        var x when x == typeof(Customer) => DataSource.data[type],
        //        var x when x == typeof(BaseStation) => DataSource.data[type],
        //        var x when x == typeof(Parcel) => DataSource.data[type],
        //        var x when x == typeof(Drone) => DataSource.data[type],
        //        _ => throw new Exception()
        //    };
        //}

    }
}
