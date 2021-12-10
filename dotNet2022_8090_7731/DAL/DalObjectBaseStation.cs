using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal.DO;
using static DalObject.DataSource;
using static DalObject.DataSource.Config;

namespace DalObject
{
    /// <summary>
    /// partial class of DalObject:IDal
    /// </summary>
    public partial class DalObject
    /// <summary>
    /// A class that contains:
    /// Add
    /// BelongingParcel
    /// PickingUpParcel
    /// DeliveryPackage
    /// ChangeDroneStatus
    /// ChargingDrone
    /// ReleasingDrone
    /// GetBaseStation
    /// GetDrone
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
        /// A function that gets a base station and adds
        /// it to the list of Base Stations,the function
        /// doesn't return anything.
        /// </summary>
        /// <param name="baseStation"></param>
        //public void AddingBaseStation(BaseStation baseStation)
        //{
        //    BaseStationList.Add(baseStation);
        //}

        public void AddingItemToDList<T>(T item) where T : IIdentifiable,IDalObject
        {
            ((List<T>)data[typeof(T)]).Add(item);
        }

        ///// <summary>
        ///// A function that gets an id of base station and returns this base station-copied.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>a base station </returns>
        ///// 
        //public BaseStation GetBaseStation(int id)
        //{
        //    for (int i = 0; i < BaseStationList.Count; i++)
        //    {
        //        if (BaseStationList[i].Id == id)
        //        {
        //            return BaseStationList[i].Clone();
        //        }
        //    }
        //    throw new IdNotExistInTheListException();
        //}

        ///// <summary>
        ///// A function that returns the list of the base stations 
        ///// </summary>
        ///// <returns> base station list</returns>
        //public IEnumerable<BaseStation> GetBaseStations()
        //{
        //    return BaseStationList.Select(station => new BaseStation(station)).ToList();
        //}

        /// <summary>
        /// A function that returns - copy of base stations
        /// that they have available charging positions.
        /// </summary>
        /// <returns>list of base stations that they have available charging positions</returns>
        //public IEnumerable<BaseStation> AvailableSlots()
        //{
        //    return new List<BaseStation>(BaseStationList.Where(baseStation => AreThereFreePositions(baseStation.Id)));
        //}

        public bool AreThereFreePositions(int sId)
        {
            return (BaseStationList.Find(baseStation => baseStation.Id == sId).NumberOfChargingPositions - SumOfDronesInSpecificStation(sId)) > 0;
        }
        
        /// <summary>
        /// A function that gets an id of base station and an 
        /// object of base station and changes
        /// the data base to contain this object to the id that is
        /// sent to the function,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="bId"></param>
        /// <param name="baseStation"></param>
        public void UpdateBaseStation(int bId, BaseStation baseStation)
        {
            BaseStationList.Remove(BaseStationList.Find(baseStation => baseStation.Id == bId));
            BaseStationList.Add(baseStation);
        }
    }
}
