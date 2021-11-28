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
        //public IEnumerable<StationToList> GetStations()
        //{
        //    //
        //    //return GetBList<StationToList, IDal.DO.BaseStation>(MapStation);

        //    //IEnumerable<StationToList> bStationsList = new List<StationToList>();
        //    //List<IDal.DO.Station> dStationsList = GetList<IDal.DO.BaseStation>();
        //    ////IEnumerable<IDal.DO.Station> dParcelList = dal.GetStations();
        //    //foreach (var station in dStationsList)
        //    //{
        //    //    bStationsList.Add(MapToList(station));
        //    //}
        //    //return bStationsList;
        //}
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
                IDal.DO.BaseStation baseStation = dal.GetFromDalById<IDal.DO.BaseStation>(stationId);
                if (!string.IsNullOrEmpty(stationName))
                    baseStation.NameStation = stationName;

                if (!string.IsNullOrEmpty(amountOfPositions))
                    baseStation.NumberOfChargingPositions = int.Parse(amountOfPositions);
                dal.UpdateBaseStation(stationId, baseStation);
            }
            catch (IDal.DO.IdNotExistInTheListException)
            {
                throw new IdIsNotValidException("Id of this base station doesn't " +
                    "exist in the base station list!!");
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
        /// A function that gets id of station and
        /// returns a new list of charging drone of BL of this station.
        /// </summary>
        /// <param name="sId"></param>
        /// <returns>a new list of charging drone of BL of specific station</returns>
        private List<ChargingDrone> ChargingDroneBLList(int sId)
        {
            var chargingDroneBLList = new List<ChargingDrone>();
            var chargingDroneDalList =dal.GetChargingDrones().Where(charge=>charge.StationId==sId);
            var chargingDrone= new ChargingDrone();
            foreach (var chargingPosition in chargingDroneDalList)
	        {
                chargingDrone.DroneId = chargingPosition.DroneId;
                chargingDrone.BatteryStatus = lDroneToList.FirstOrDefault(drone=>drone.Id== chargingDrone.DroneId).BatteryStatus;
	            chargingDroneBLList.Add(chargingDrone);
            }
            return chargingDroneBLList;
        }

        //private Station ConvertToBL(IDal.DO.BaseStation station)
        //{
        //    var nLocation = new Location(station.Longitude, station.Latitude);
        //    var numAvailablePositions = station.NumberOfChargingPositions - MountOfFullPositions(nLocation);
        //    var chargingDroneBList = ChargingDroneBLList(); 
        //    return new Station(station.Id, station.NameStation, nLocation, numAvailablePositions, chargingDroneBList);
        //}

        /// <summary>
        /// A function that gets an object of IDal.DO.BaseStation
        /// and expands it to StationToList object and returns it.
        /// </summary>
        /// <param name="station"></param>
        /// <returns>returns StationToList object</returns>
        private StationToList ConvertToList(IDal.DO.BaseStation station)
        {
            StationToList nStation = new StationToList();
            nStation.Id = station.Id;
            nStation.Name = station.NameStation;
            nStation.FullPositions = MountOfFullPositions(new Location(station.Longitude, station.Latitude));
            nStation.AvailablePositions = station.NumberOfChargingPositions - nStation.FullPositions;
            return nStation;
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
                if (drone.DStatus == DroneStatus.Maintenance && equalLocations(drone.CurrLocation, stationLocation))
                {
                    ++sumFullPositions;
                }
            }
            return sumFullPositions;
        }
        /// <summary>
        /// A function that gets two locations and returns if they are the same or not.
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <returns>returns if they are the same or not</returns>
        private bool equalLocations( Location location1, Location location2 )
        {
            return location1.Longitude == location2.Longitude && location1.Latitude == location2.Latitude;
        }
        /// <summary>
        /// A function that gets a location and retrns the
        /// closet base station-BL to this location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>returns Station that closet to the location that the function gets.</returns>
        private Station ClosestStation(Location location)
        {
            var stationDalList = dal.GetListFromDal<IDal.DO.BaseStation>();
            var cCoord = new GeoCoordinate(location.Latitude, location.Longitude);
            var sCoord = new GeoCoordinate(stationDalList.ElementAt(0).Latitude, stationDalList.ElementAt(0).Longitude);
            double currDistance, distance = sCoord.GetDistanceTo(cCoord);
            int index = 0;
            for (int i = 1; i < stationDalList.Count(); i++)
            {
                sCoord = new GeoCoordinate(stationDalList.ElementAt(i).Latitude, stationDalList.ElementAt(i).Longitude);
                currDistance = sCoord.GetDistanceTo(cCoord);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    index = i;
                }
            }
            return ConvertToBL(stationDalList.ElementAt(index));
        }
        /// <summary>
        /// A function that gets a base station and adds it to the data base,
        /// the function doesn't return anything.
        /// </summary>
        /// <param name="bLStation"></param>
        public void AddingBaseStation(Station bLStation)
        {
            if (dal.IsIdExistInList<IDal.DO.BaseStation>(bLStation.Id))
            {
                throw new IdIsNotValidException("The id is already exists in the base station list!");
            }
            IDal.DO.BaseStation station = new IDal.DO.BaseStation() {
            Id = bLStation.Id,
            Latitude = bLStation.SLocation.Latitude,
            Longitude = bLStation.SLocation.Longitude,
            NameStation = bLStation.NameStation,
            NumberOfChargingPositions = bLStation.NumAvailablePositions };
            dal.AddingItemToDList(station);
        }
    }
}
