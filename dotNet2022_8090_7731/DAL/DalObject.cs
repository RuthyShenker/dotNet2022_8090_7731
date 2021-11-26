using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using IDal.DO;
using static DalObject.DataSource;
using static DalObject.DataSource.Config;

namespace DalObject
{
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
            Initialize();
        }

        /// <summary>
        /// A function that gets a base station and adds it to the list of Base Stations.
        /// </summary>
        /// <param name="baseStation"></param>
        public void AddingBaseStation(BaseStation baseStation)
        {
            BaseStationList.Add(baseStation);
        }
        public int PickingUpAndReturnIndexParcel()
        {
            return ++IndexParcel;
        }
        /// <summary>
        /// A function that gets an id of base station and returns this base station-copied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a base station </returns>
        /// 
        public BaseStation BaseStationDisplay(int id)
        {
            for (int i = 0; i < BaseStationList.Count; i++)
            {
                if (BaseStationList[i].Id == id)
                {
                    return BaseStationList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
        }

        /// <summary>
        /// A function that returns the list of the base stations 
        /// </summary>
        /// <returns> base station list</returns>
        public IEnumerable<BaseStation> GetBaseStations()
        {
            return BaseStationList.Select(station => new BaseStation(station)).ToList();
        }

        /// <summary>
        /// A function that returns base stations that they have available charging positions.
        /// </summary>
        /// <returns>list of base stations that they have available charging positions</returns>
        public IEnumerable<BaseStation> AvailableSlots()
        {
            return new List<BaseStation>(BaseStationList.Where(BaseStation => BaseStation.NumberOfChargingPositions > 0).ToList());
        }



        Customer IDal.IDal.CustomerDisplay(string Id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Customer> IDal.IDal.DisplayingCustomers()
        {
            throw new NotImplementedException();
        }

        public bool ExistsInBaseStation(int id)
        {
            for (int i = 0; i < BaseStationList.Count; i++)
            {
                if (BaseStationList[i].Id == id)
                    return true;
            }
            return false;
        }

        public bool ExistsInDroneList(int id)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (DroneList[i].Id==id)
                    return true;
            }
            return false;
        }
        public bool IsIdExistInList<T>(int Id) where T : IIdentifiable
        {
            return ((List<T>)data[typeof(T)]).Any(item => item.Id == Id);
        }
        public void AddDroneToCharge(int dId, int sId)
       {
            ChargingDroneList.Add(new ChargingDrone(dId,sId));
       }

        public T GetFromDalById<T>(int Id) where T : IIdentifiable
        {
            try
            {
                return ((List<T>)data[typeof(T)]).Find(item => item.Id == Id);
            }
            catch (ArgumentNullException exception)
            {
                throw new IdNotExistInTheListException();
            }
        }

        public T GetFromDalByCondition<T>(Predicate<T> predicate) where T : IIdentifiable
        {
            return ((List<T>)data[typeof(T)]).Find(predicate);
        }
        public IEnumerable<T> GetDalListByCondition<T>(Predicate<T> predicate) where T : IIdentifiable
        {
            return ((List<T>)data[typeof(T)]).FindAll(predicate);
        }
        IEnumerable<T> GetListFromDal<T>() where T:IIdentifiable
        {
            return (IEnumerable<T>)data[typeof(T)];
        }

        public bool IsExistInList<T>(List<T> list, Predicate<T> predicate)
        {
            return list.Find(predicate).Equals(default(T));
        }
    }
}
