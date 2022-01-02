using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

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
        public void AddingBaseStation(int id, string name, double longitude, double latitude, int numPositions)
        {
            if (dal.IsIdExistInList<DO.BaseStation>(id))
            {
                throw new IdIsAlreadyExistException(typeof(DO.BaseStation), id);
            }
            DO.BaseStation station = new DO.BaseStation()
            {
                Id = id,
                Latitude = latitude,
                Longitude = longitude,
                NameStation = name,
                NumberOfChargingPositions = numPositions
            };
            dal.Add(station);
        }

        /// <summary>
        /// A function that gets stationId,stationName,amountOfPositions
        /// and updates the station with the stationId to be with these fields
        /// and updates it in the data base,the function doesn't return anything.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="stationName"></param>
        /// <param name="amountOfPositions"></param>
        public void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions)
        {
            try
            {
                var baseStation = dal.GetFromDalById<DO.BaseStation>(stationId);

                if (!string.IsNullOrEmpty(stationName))
                    dal.Update<DO.BaseStation>(stationId, stationName, nameof(baseStation.NameStation));

                if (amountOfPositions > 0)
                {
                    dal.Update<DO.BaseStation>(stationId, amountOfPositions, nameof(baseStation.NumberOfChargingPositions));
                }
                //else  TODO  not always must update two fields? can change exception
                //{
                //    throw new InValidActionException("number of positions cannot be lower than 1");
                //}
            }
            catch (DO.IdIsNotExistException)
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

            var chargingDroneBLList = Enumerable.Empty<ChargingDrone>();
            var chargingDroneDalList = dal.GetDalListByCondition<DO.ChargingDrone>(charge => charge.StationId == sId);
            var chargingDrone = new ChargingDrone();
            foreach (var chargingPosition in chargingDroneDalList)
            {
                chargingDrone.DroneId = chargingPosition.DroneId;
                chargingDrone.BatteryStatus = lDroneToList.FirstOrDefault(drone => drone.Id == chargingDrone.DroneId).BatteryStatus;
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
        private Station ClosestStation(Location location, bool sendingToCharge = false)
        {
            var cCoord = new GeoCoordinate(location.Latitude, location.Longitude);
            var stationDalList = dal.GetListFromDal<DO.BaseStation>();
            GeoCoordinate sCoord;
            double currDistance, minDistance = double.MaxValue;
            DO.BaseStation closetStation = stationDalList.ElementAt(0);
            for (int i = 0; i < stationDalList.Count(); ++i)
            {
                sCoord = new GeoCoordinate(stationDalList.ElementAt(i).Latitude, stationDalList.ElementAt(i).Longitude);
                currDistance = sCoord.GetDistanceTo(cCoord);
                if (currDistance < minDistance)
                {
                    if (!sendingToCharge)
                    {
                        minDistance = currDistance;
                        closetStation = stationDalList.ElementAt(i);
                    }
                    else if (GetNumOfAvailablePositionsInStation(stationDalList.ElementAt(i).Id) > 0)
                    {
                        minDistance = currDistance;
                        closetStation = stationDalList.ElementAt(i);
                    }
                }
            }
            return ConvertToBL(closetStation);
        }

        public string DeleteStation(int stationId)
        {
            try
            {
                dal.Remove(dal.GetFromDalById<DO.BaseStation>(stationId));
            }
            catch (DO.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(DO.BaseStation), stationId);
            }
            return $"Station with Id: {stationId} was successfully removed from the system";
        }

        //-----Get-----------------------------------------------

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
        public IEnumerable<StationToList> AvailableSlots(int numPositions = 0)
        {
            return numPositions == 0
                ? dal.GetDalListByCondition<DO.BaseStation>(baseStation => GetNumOfAvailablePositionsInStation(baseStation.Id) > 0)
                 .Select(station => ConvertToList(station))

                : dal.GetDalListByCondition<DO.BaseStation>(baseStation => GetNumOfAvailablePositionsInStation(baseStation.Id) == numPositions)
                     .Select(station => ConvertToList(station));
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
            StationToList nStation = new(
                station.Id,
                station.NameStation,
                numOfAvailablePositions,
                station.NumberOfChargingPositions - numOfAvailablePositions);
            return nStation;
        }

        public Station GetStation(int stationId)
        {
            try
            {
                var dStation = dal.GetFromDalById<DO.BaseStation>(stationId);
                return ConvertToBL(dStation);
            }
            catch (DO.IdIsNotExistException)
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
            var nLocation = new Location(station.Longitude, station.Latitude);
            var numAvailablePositions = GetNumOfAvailablePositionsInStation(station.Id);
            var chargingDroneBList = ChargingDroneBLList(station.Id);
            return new Station(station.Id, station.NameStation, nLocation, numAvailablePositions, chargingDroneBList);
        }

        private int GetNumOfAvailablePositionsInStation(int stationId)
        {
            var station = dal.GetFromDalById<DO.BaseStation>(stationId);
            int numOfChargingDroneInStation = dal.GetDalListByCondition<DO.ChargingDrone>(s => s.StationId == stationId).Count();
            return station.NumberOfChargingPositions - numOfChargingDroneInStation;
        }
    }
}
