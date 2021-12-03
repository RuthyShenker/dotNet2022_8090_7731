using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

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
            if (dal.IsIdExistInList<IDal.DO.BaseStation>(bLStation.Id))
            {
                throw new DalObject.IdIsAlreadyExistException(typeof(IDal.DO.BaseStation), bLStation.Id);
            }
            IDal.DO.BaseStation station = new IDal.DO.BaseStation()
            {
                Id = bLStation.Id,
                Latitude = bLStation.SLocation.Latitude,
                Longitude = bLStation.SLocation.Longitude,
                NameStation = bLStation.NameStation,
                NumberOfChargingPositions = bLStation.NumAvailablePositions
            };
            dal.AddingItemToDList(station);
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
                var baseStation = dal.GetFromDalById<IDal.DO.BaseStation>(stationId);

                if (!string.IsNullOrEmpty(stationName))
                    baseStation.NameStation = stationName;
                if (!string.IsNullOrEmpty(amountOfPositions))
                    baseStation.NumberOfChargingPositions = int.Parse(amountOfPositions);

                dal.UpdateBaseStation(stationId, baseStation);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDal.DO.BaseStation), stationId);
            }
        }


        /// <summary>
        /// A function that gets an object of IDal.DO.BaseStation
        /// and expands it to Station type and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns Station object</returns>
        private Station ConvertToBL(IDal.DO.BaseStation station)
        {
            var nLocation = new Location(station.Longitude, station.Latitude);
            var numAvailablePositions = station.NumberOfChargingPositions - MountOfFullPositions(nLocation);
            var chargingDroneBList = ChargingDroneBLList(station.Id);
            return new Station(station.Id, station.NameStation, nLocation, numAvailablePositions, chargingDroneBList);
        }

        /// <summary>
        /// A function that gets an object of IDal.DO.BaseStation
        /// and expands it to StationToList object and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns StationToList object</returns>
        private StationToList ConvertToList(IDal.DO.BaseStation station)
        {
            var fullPositions = MountOfFullPositions(new Location(station.Longitude, station.Latitude));
            StationToList nStation = new(
            station.Id,
            station.NameStation,
            station.NumberOfChargingPositions - fullPositions,
            fullPositions);
            return nStation;
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
            var chargingDroneDalList = dal.GetChargingDrones().Where(charge => charge.StationId == sId);
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
        /// A function that returns Available Slots by type of StationToList.
        /// </summary>
        /// <returns> returns Available Slots</returns>
        public IEnumerable<StationToList> AvailableSlots()
        {
            var stationsDalList = dal.AvailableSlots();
            var stationBalList = new List<StationToList>();
            foreach (var station in stationsDalList)
            {
                stationBalList.Add(ConvertToList(station));
            }
            return stationBalList;
        }

        /// <summary>
        /// A function that gets a location of station 
        /// and returns Mount Of Full Positions of this station.
        /// </summary>
        /// <param name="stationLocation"></param>
        /// <returns>ount Of Full Positions of specific station</returns>
        private int MountOfFullPositions(Location stationLocation)
        {
            int sumFullPositions = 0;
            foreach (var drone in lDroneToList)
            {
                if (drone.DStatus == DroneStatus.Maintenance && EqualLocations(drone.CurrLocation, stationLocation))
                {
                    ++sumFullPositions;
                }
            }
            return sumFullPositions;
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
            var stationDalList = dal.GetListFromDal<IDal.DO.BaseStation>();
            var cCoord = new GeoCoordinate(location.Latitude, location.Longitude);
            var sCoord = new GeoCoordinate(stationDalList.ElementAt(0).Latitude, stationDalList.ElementAt(0).Longitude);
            double currDistance, distance = sCoord.GetDistanceTo(cCoord);
            int index = 0;
            for (int i = 1; i < stationDalList.Count(); ++i)
            {
                sCoord = new GeoCoordinate(stationDalList.ElementAt(i).Latitude, stationDalList.ElementAt(i).Longitude);
                currDistance = sCoord.GetDistanceTo(cCoord);
                if (currDistance < distance)
                {
                    if (!sendingToCharge)
                    {
                        distance = currDistance;
                        index = i;
                    }
                    else if (dal.AreThereFreePositions(stationDalList.ElementAt(i).Id))
                    {
                        distance = currDistance;
                        index = i;
                    }
                }
            }
            return ConvertToBL(stationDalList.ElementAt(index));
        }
    }
}
