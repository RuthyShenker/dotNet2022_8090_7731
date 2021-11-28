﻿using System;
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

        private Station ConvertToBL(IDal.DO.BaseStation station)
        {
            var nLocation = new Location(station.Longitude, station.Latitude);
            var numAvailablePositions = station.NumberOfChargingPositions - MountOfFullPositions(nLocation);
            var chargingDroneBList = ChargingDroneBLList(station.Id);
            return new Station(station.Id, station.NameStation, nLocation, numAvailablePositions, chargingDroneBList);
        }

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


        // returns a new list of charging drone of BL
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

        private StationToList ConvertToList(IDal.DO.BaseStation station)
        {
            StationToList nStation = new StationToList();
            nStation.Id = station.Id;
            nStation.Name = station.NameStation;
            nStation.FullPositions = MountOfFullPositions(new Location(station.Longitude, station.Latitude));
            nStation.AvailablePositions = station.NumberOfChargingPositions - nStation.FullPositions;
            return nStation;
        }

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

        private bool equalLocations( Location location1, Location location2 )
        {
            return location1.Longitude == location2.Longitude && location1.Latitude == location2.Latitude;
        }
        
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
