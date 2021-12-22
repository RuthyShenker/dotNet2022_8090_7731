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
        public void AddingBaseStation(Station bLStation)
        {
            if (dal.IsIdExistInList<DO.BaseStation>(bLStation.Id))
            {
                throw new IdIsAlreadyExistException(typeof(DO.BaseStation), bLStation.Id);
            }
            DO.BaseStation station = new DO.BaseStation()
            {
                Id = bLStation.Id,
                Latitude = bLStation.Location.Latitude,
                Longitude = bLStation.Location.Longitude,
                NameStation = bLStation.NameStation,
                NumberOfChargingPositions = bLStation.NumAvailablePositions
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
        public void UpdatingStationDetails(int stationId, string stationName, string amountOfPositions)
        {
            try
            {
                var baseStation = dal.GetFromDalById<DO.BaseStation>(stationId);

                if (!string.IsNullOrEmpty(stationName))
                    dal.Update<DO.BaseStation>(stationId, stationName, nameof(baseStation.NameStation));

                if (!string.IsNullOrEmpty(amountOfPositions))
                    dal.Update<DO.BaseStation>(stationId, int.Parse(amountOfPositions), nameof(baseStation.NumberOfChargingPositions));
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
        private List<ChargingDrone> ChargingDroneBLList(int sId)
        {
            var chargingDroneBLList = new List<ChargingDrone>();
            var chargingDroneDalList = dal.GetDalListByCondition<DO.ChargingDrone>(charge => charge.StationId == sId);
            var chargingDrone = new ChargingDrone();
            foreach (var chargingPosition in chargingDroneDalList)
            {
                chargingDrone.DroneId = chargingPosition.DroneId;
                chargingDrone.BatteryStatus = lDroneToList.FirstOrDefault(drone => drone.Id == chargingDrone.DroneId).BatteryStatus;
                chargingDroneBLList.Add(chargingDrone);
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
            double currDistance, minDistance=double.MaxValue;
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
                        closetStation= stationDalList.ElementAt(i);
                    }
                    else if (GetNumOfAvailablePositionsInStation(stationDalList.ElementAt(i).Id) > 0)
                    {
                        minDistance = currDistance;
                        closetStation =  stationDalList.ElementAt(i);
                    }
                }
            }
            return ConvertToBL(closetStation);
        }

        //-----Get-----------------------------------------------

        public IEnumerable<StationToList> GetStations()
        {
            return dal.GetListFromDal<DO.BaseStation>()
                .Select(station => ConvertToList(station));
        }

        /// <summary>
        /// A function that returns Available Slots by type of StationToList.
        /// </summary>
        /// <returns> returns Available Slots</returns>
        public IEnumerable<StationToList> AvailableSlots()
        {
           return  dal.GetDalListByCondition<DO.BaseStation>(baseStation => GetNumOfAvailablePositionsInStation(baseStation.Id) > 0)
                .Select(station=> ConvertToList(station));
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
            int numOfChargingDroneInStation = dal.GetDalListByCondition<DO.ChargingDrone>(s=>s.StationId==stationId).Count();
            return station.NumberOfChargingPositions - numOfChargingDroneInStation;
        }
    }
}
