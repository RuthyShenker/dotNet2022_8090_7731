﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDAL.DO;
using static DalObject.DataSource;
using static DalObject.DataSource.Config;

namespace DalObject
{
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
        /// A function that gets a base station and adds it to the list of Base Stations.
        /// </summary>
        /// <param name="baseStation"></param>
        public void AddingBaseStation(BaseStation baseStation)
        {
            BaseStationList.Add(baseStation);
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
        /// A function that returns copy base stations that they have available charging positions.
        /// </summary>
        /// <returns>list of base stations that they have available charging positions</returns>
        public IEnumerable<BaseStation> AvailableSlots()
        {
            return new List<BaseStation>(BaseStationList.Where(BaseStation => BaseStation.NumberOfChargingPositions > 0).ToList());


            //צריך גםTOLISTוגם NEW LIST??????
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

        
        public bool ThereAreFreePositions(int sId)
        {
            return (BaseStationList.Find(baseStation => baseStation.Id == sId).NumberOfChargingPositions - SumOfDronesInSpecificStation(sId)) > 0;
        }

        public void UpdateBaseStation(int bId, BaseStation baseStation)
        {
            try
            {
                BaseStationList.Remove(BaseStationList.Find(baseStation => baseStation.Id == bId));
                BaseStationList.Add(baseStation);
            }
            catch (ArgumentNullException )
            {
                throw new IdNotExistInTheListException();
            }
        }


    }
}
