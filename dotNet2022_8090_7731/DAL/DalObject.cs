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
        public DalObject()
        {
            //TODO:
            //Initialize();
            
            data = new()
            {
                [typeof(Drone)] = DroneList,
                [typeof(Customer)] = CustomerList,
                [typeof(Parcel)] = ParceList,
                [typeof(BaseStation)] = BaseStationList,
                [typeof(ChargingDrone)] = ChargingDroneList,
            };
        }

        //Update Generic:
        //void Update<T>(int id, T obj) where T : IDalObject, IIdentifiable
        //{
        //    data[typeof(T)].Remove(((List<T>)data[typeof(T)]).Find(Object => Object.Id == id));
        //    data[typeof(T)].Add(obj);
        //}

        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable,IDalObject
        {
            return ((List<T>)data[typeof(T)]).Any(item => item.Id == Id);
        }
       
        public T GetFromDalById<T>(int Id) where T : IDalObject,IIdentifiable
        {
            try
            {
                ((List<T>)data[typeof(T)]).First(item => item.Id == Id);
                return GetFromDalByCondition<T>(item => item.Id == Id);
            }
            catch(InvalidOperationException)
            {
                throw new IdIsNotExistException();
            }
        }

        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T: IDalObject
        {
            return ((List<T>)data[typeof(T)]).Find(predicate);
        }

        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T: IDalObject
        {
            return new List<T>(((List<T>)data[typeof(T)]).FindAll(predicate));
        }

        public IEnumerable<T> GetListFromDal<T>() where T:IDalObject
        {
            return new List<T>((List<T>)data[typeof(T)]);
        }

        //public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)where T:IDalObject
        //{
        //    return list.Find(predicate).Equals(default(T));
        //}
    }
}
