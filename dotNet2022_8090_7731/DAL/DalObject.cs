using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal.DO;
using static DalObject.DataSource;

namespace DalObject
{
    /// <summary>
    /// partial class of DalObject:IDal
    /// </summary>
    public partial class DalObject : IDal.IDal
    /// <summary>
    /// A class that contains:
    /// Add
    /// BelongingParcel
    /// PickingUpParcel
    /// DeliveryPackage
    /// ChangeDroneStatus
    /// ChargingDrone
    /// ReleasingDrone
    /// BaseStationDisplay
    /// DroneDisplay
    /// CustomerDisplay
    /// ParcelDisplay
    /// GetBaseStations
    /// DisplayingDrones
    /// DisplayingCustomers
    /// DisplayingParcels
    /// DisplayingUnbelongParcels
    /// AvailableSlots
    /// </summary>
    {
        /// <summary>
        /// A constructor of DalObject that activates the function Initialize
        /// </summary>
        //public DalObject()
        //{
        //    Initialize();
        //}

       
        ///// <summary>
        ///// A function that gets an id of base station and returns this base station-copied.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>a base station </returns>
        ///// 
        //public BaseStation BaseStationDisplay(int id)
        //{
        //    for (int i = 0; i < BaseStationList.Count; i++)
        //    {
        //        if (BaseStationList[i].Id == id)
        //        {
        //            return BaseStationList[i].Clone();
        //        }
        //    }
        //    throw new Exception("id doesnt exist");
        //}

        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable
        {
            return ((List<T>)data[typeof(T)]).Any(item => item.Id == Id);
        }
       
        public T GetFromDalById<T>(int Id) where T : IIdentifiable
        {

            return ((List<T>)data[typeof(T)]).First(item => item.Id == Id);

        }

        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IIdentifiable
        {
            return ((List<T>)data[typeof(T)]).Find(predicate);
        }
        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IIdentifiable
        {
            return new List<T>(((List<T>)data[typeof(T)]).FindAll(predicate));
        }

        

        public IEnumerable<T> GetListFromDal<T>() where T:IIdentifiable
        {
            return new List<T>((IEnumerable<T>)data[typeof(T)]);
        }

        public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)
        {
            return list.Find(predicate).Equals(default(T));
        }
    }
}
