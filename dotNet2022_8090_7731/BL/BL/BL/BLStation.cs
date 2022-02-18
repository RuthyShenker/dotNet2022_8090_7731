using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    /// <summary>
    /// An internal sealed partial class BL inherits from Singleton<BL>,and impliments BlApi.IBL,
    /// </summary>
    partial class BL : BlApi.IBL
    {
        /// <summary>
        /// A function that gets a base station and adds it to the data base,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="bLStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddingBaseStation(BO.Station blStation)
        {
            lock (dal)
            {
                if (dal.IsIdExistInList<DO.BaseStation>(blStation.Id))
                {
                    throw new IdAlreadyExistsException(typeof(DO.BaseStation), blStation.Id);
                }

                DO.BaseStation dlStation = new()
                {
                    Id = blStation.Id,
                    Latitude = blStation.Location.Latitude,
                    Longitude = blStation.Location.Longitude,
                    NameStation = blStation.NameStation,
                    NumberOfChargingPositions = blStation.NumAvailablePositions
                };
                dal.Add(dlStation);
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
        /// A function that gets id of station and deletes it from the db.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>returns string that the action performs successfully </returns>
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
        /// <summary>
        /// A function that returns all the Stations from the db after converts them to bl type.
        /// </summary>
        /// <returns>returns all the Stations from the db after converts them to bl type.</returns>
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
        /// A function that gets station id and returns the station with this id after converts it to BL.Station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>returns the station with this id after converts it to BL.Station</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            DO.BaseStation dStation;
            try
            {
                lock (dal)
                {
                    dStation = dal.GetFromDalById<DO.BaseStation>(stationId);
                }
                return ConvertToBL(dStation);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Station), stationId);
            }
        }

        /// <summary>
        /// A function that gets a location and retrns the
        /// closet base station-BL to this location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>
        /// returns the station with the closest location to the gotten location
        /// if sending to charge is true- return the station with the closest location which has free slots to charge in,
        /// otherwise the first station in the list-the -the closet station even if it doesn't have free slots.
        internal Station ClosestStation(Location location, bool sendingToCharge = false)
        {
            // TODO catch when the list is empty or there is no first...
            try
            {
                lock (dal)
                {
                    var droneCoord = Extensions.geoCoordinate(location); //new GeoCoordinate(location.Latitude, location.Longitude);

                    var closetStation = dal.GetListFromDal<DO.BaseStation>()
                        .OrderBy(station => Extensions.geoCoordinate(new() { Latitude = station.Latitude, Longitude = station.Longitude }).GetDistanceTo(droneCoord)).First();

                    if (!sendingToCharge)
                    {
                        return ConvertToBL(closetStation);
                    }

                    // if closet station is full, releasing the most charged drone from the station
                    if (GetNumOfAvailablePositionsInStation(closetStation.Id) == 0)
                    {
                        var mostChargedDrone = ChargingDroneBLList(closetStation.Id).OrderByDescending(s => s.BatteryStatus).First();
                        ReleasingDrone(mostChargedDrone.DroneId);
                    }

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

        /// <summary>
        /// A function that gets an object of IDAL.DO.BaseStation
        /// and expands it to StationToList object and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns StationToList instance</returns>
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

        /// <summary>
        /// A function that gets an instance of IDAL.DO.BaseStation
        /// and convets it to BO.Station type and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns Station instance</returns>
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

        /// <summary>
        /// A function that gets id of station and returns the Number Of Available Positions In this Station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>returns the Number Of Available Positions In this Station</returns>
        private int GetNumOfAvailablePositionsInStation(int stationId)
        {
            try
            {
                DO.BaseStation station;
                int numOfChargingDroneInStation;
                lock (dal)
                {

                    station = dal.GetFromDalById<DO.BaseStation>(stationId);
                    numOfChargingDroneInStation = dal.GetDalListByCondition<DO.ChargingDrone>(s => s.StationId == stationId).Count();


                }
                return station.NumberOfChargingPositions - numOfChargingDroneInStation;
            }
            catch (ArgumentNullException)
            {
                throw new ListIsEmptyException(typeof(BO.Station));
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
            try
            {
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
                    chargingDroneBLList.Append(chargingDrone);
                }
            }
            catch (ArgumentNullException)
            {
                throw new ListIsEmptyException();
            }
            return chargingDroneBLList;

        }

    }
}
