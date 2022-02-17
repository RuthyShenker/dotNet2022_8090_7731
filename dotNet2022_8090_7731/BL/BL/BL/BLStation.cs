﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// A function that gets a base station and adds it to the data base,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="bLStation"></param>
        //public void AddingBaseStation(Station bLStation)
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddingBaseStation(int id, string name, double longitude, double latitude, int numPositions)
        {
            lock (dal)
            {
                if (dal.IsIdExistInList<DO.BaseStation>(id))
                {
                    throw new IdIsAlreadyExistException(typeof(DO.BaseStation), id);
                }
                DO.BaseStation station = new()
                {
                    Id = id,
                    Latitude = latitude,
                    Longitude = longitude,
                    NameStation = name,
                    NumberOfChargingPositions = numPositions
                };
                dal.Add(station);
            }
        }

        /// <summary>
        /// A function that gets stationId,stationName,amountOfPositions
        /// and updates the station with the stationId to be with these fields
        /// and updates it in the data base,the function doesn't return anything.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="stationName"></param>
        /// <param name="amountOfPositions"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions)
        {
            try
            {
                lock (dal)
                {
                    var baseStation = dal.GetFromDalById<DO.BaseStation>(stationId);

                    if (!string.IsNullOrEmpty(stationName))
                        dal.Update<DO.BaseStation>(stationId, stationName, nameof(baseStation.NameStation));

                    if (amountOfPositions > 0)
                    {
                        dal.Update<DO.BaseStation>(stationId, amountOfPositions, nameof(baseStation.NumberOfChargingPositions));
                    }
                }
                //else  TODO  not always must update two fields? can change exception
                //{
                //    throw new InValidActionException("number of positions cannot be lower than 1");
                //}
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(DO.BaseStation), stationId);
            }
        }

        /// <summary>
        /// A function that gets id of station and
        /// returns a new list of charging drone of BL of this station.
        /// </summary>
        /// <param name="sId"></param>
        /// <returns>a new list of charging drone of BL of specific station</returns>
        private IEnumerable<ChargingDrone> ChargingDroneBLList(int sId)
        {
            IEnumerable<DO.ChargingDrone> chargingDroneDalList;
            ChargingDrone chargingDrone;
            var chargingDroneBLList = Enumerable.Empty<ChargingDrone>();
            lock (dal)
            {
                chargingDroneDalList = dal.GetDalListByCondition<DO.ChargingDrone>(charge => charge.StationId == sId);
            }
            foreach (var chargingPosition in chargingDroneDalList)
            {
                chargingDrone = new()
                {
                    DroneId = chargingPosition.DroneId,
                    BatteryStatus = lDroneToList.FirstOrDefault(drone => drone.Id == chargingPosition.DroneId)?.BatteryStatus ?? 0
                };
                // TODO: the problem was that there werent use in the return Append. to check
                // if there more places like that.
                chargingDroneBLList = chargingDroneBLList.Append(chargingDrone);
            }
            return chargingDroneBLList;
        }

        /// <summary>
        /// A function that gets a location and retrns the
        /// closet base station-BL to this location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>returns Station that closet to the location that the function gets.</returns>
        /// //
        // return the station with the closest location to the gotten location
        // if sending to charge is true- return the station with the closest location which is has free slots to charge in,
        // otherwise the first station in the list.
        internal Station ClosestStation(Location location, bool sendingToCharge = false)
        {
            // TODO catch when the list is empty or there is no first...
            try
            {
                lock (dal)
                {
                    var droneCoord = new GeoCoordinate(location.Latitude, location.Longitude);

                    var sortedList = dal.GetListFromDal<DO.BaseStation>()
                        .OrderBy(station => new GeoCoordinate(station.Latitude, station.Longitude).GetDistanceTo(droneCoord));

                    var closetStation = sendingToCharge == true
                        ? sortedList.FirstOrDefault(s => GetNumOfAvailablePositionsInStation(s.Id) > 0) : sortedList.First();

                    return ConvertToBL(closetStation);
                }
            }
            catch (ArgumentNullException)
            {
                throw new ListIsEmptyException();
            }
            #region canDelete
            //var cCoord = new GeoCoordinate(location.Latitude, location.Longitude);
            //var stationDalList = dal.GetListFromDal<DO.BaseStation>();

            //GeoCoordinate sCoord;
            //double currDistance, minDistance = double.MaxValue;
            //DO.BaseStation closetStation = stationDalList.ElementAt(0);
            //for (int i = 0; i < stationDalList.Count(); ++i)
            //{
            //    sCoord = new GeoCoordinate(stationDalList.ElementAt(i).Latitude, stationDalList.ElementAt(i).Longitude);
            //    currDistance = sCoord.GetDistanceTo(cCoord);
            //    if (currDistance < minDistance)
            //    {
            //        if (!sendingToCharge)
            //        {
            //            minDistance = currDistance;
            //            closetStation = stationDalList.ElementAt(i);
            //        }
            //        else if (GetNumOfAvailablePositionsInStation(stationDalList.ElementAt(i).Id) > 0)
            //        {
            //            minDistance = currDistance;
            //            closetStation = stationDalList.ElementAt(i);
            //        }
            //    }
            //}
            #endregion
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string DeleteStation(int stationId)
        {
            try
            {
                lock (dal)
                {
                    dal.Remove(dal.GetFromDalById<DO.BaseStation>(stationId));
                }
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(DO.BaseStation), stationId);
            }
            return $"Station with Id: {stationId} was successfully removed from the system";
        }

        //-----Get-----------------------------------------------

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStations()
        {
            return dal.GetListFromDal<DO.BaseStation>()
                .Select(station => ConvertToList(station));
        }

        /// <summary>
        /// if numPositions == 0 returns available slots,
        /// else returns the stations which num of available positions == numPositions
        /// the return list is typeof StationToList.
        /// </summary>
        /// <param name="numPositions"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> AvailableSlots(int numPositions = 0)
        {
            lock (dal)
            {
                return numPositions == 0
                    ? dal.GetDalListByCondition<DO.BaseStation>(baseStation => GetNumOfAvailablePositionsInStation(baseStation.Id) > 0)
                     .Select(station => ConvertToList(station))

                    : dal.GetDalListByCondition<DO.BaseStation>(baseStation => GetNumOfAvailablePositionsInStation(baseStation.Id) == numPositions)
                         .Select(station => ConvertToList(station));
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.BaseStation
        /// and expands it to StationToList object and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns StationToList object</returns>
        private StationToList ConvertToList(DO.BaseStation station)
        {
            var numOfAvailablePositions = GetNumOfAvailablePositionsInStation(station.Id);
            StationToList nStation = new()
            {
                Id = station.Id,
                Name = station.NameStation,
                AvailablePositions = numOfAvailablePositions,
                FullPositions = station.NumberOfChargingPositions - numOfAvailablePositions
            };

            return nStation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            try
            {
                lock (dal)
                {
                    var dStation = dal.GetFromDalById<DO.BaseStation>(stationId);
                    return ConvertToBL(dStation);
                }
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Station), stationId);
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.BaseStation
        /// and expands it to Station type and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns Station object</returns>
        private Station ConvertToBL(DO.BaseStation station)
        {
            if (station.Equals(default))
            {
                return new Station();
            }
            var nLocation = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };
            var numAvailablePositions = GetNumOfAvailablePositionsInStation(station.Id);
            var chargingDroneBList = ChargingDroneBLList(station.Id);
            return new Station() { Id = station.Id, NameStation = station.NameStation, Location = nLocation, NumAvailablePositions = numAvailablePositions, LBL_ChargingDrone = chargingDroneBList };
        }

        private int GetNumOfAvailablePositionsInStation(int stationId)
        {
            lock (dal)
            {
                var station = dal.GetFromDalById<DO.BaseStation>(stationId);
                int numOfChargingDroneInStation = dal.GetDalListByCondition<DO.ChargingDrone>(s => s.StationId == stationId).Count();
                return station.NumberOfChargingPositions - numOfChargingDroneInStation;
            }
        }
    }
}
