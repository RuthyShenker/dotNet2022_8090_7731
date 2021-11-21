using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDal.DO;

namespace BL
{
    partial class BL
    {
        public IEnumerable<StationToList> GetStations()
        {
            //
            return GetBList<StationToList, IDal.DO.BaseStation>(MapStation);

            //IEnumerable<StationToList> bStationsList = new List<StationToList>();
            //List<IDal.DO.Station> dStationsList = GetList<IDal.DO.BaseStation>();
            ////IEnumerable<IDal.DO.Station> dParcelList = dal.GetStations();
            //foreach (var station in dStationsList)
            //{
            //    bStationsList.Add(MapToList(station));
            //}
            //return bStationsList;
        }
        public void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions)
        {
            if (!dal.ExistsInBaseStation(stationId))
            {
                throw new Exception("this id doesnt exist in base station list!");
            }
            BaseStation baseStation = dal.GetBaseStation(stationId);
            if (!string.IsNullOrEmpty(stationName))
            {
                baseStation.NameStation = stationName;
            }
            if (amountOfPositions != default)
            {
                baseStation.NumberOfChargingPositions = amountOfPositions;
            }
            dal.UpdateBaseStation(stationId, baseStation);
        }

        private StationToList MapStation(BaseStation source)
        {
            return new StationToList() { Id = source.Id };
        }

        public IEnumerable<Station> AvailableSlots()
        {
            IEnumerable<StationToList> stationsList = GetStations();
            foreach (var station in stationsList)
            {
                if (station.AvailablePositions == 0)
                {
                    stationsList.remove(station);
                }
            }
            return stationsList;
        }

        private StationToList MapToList(IDal.DO.BaseStation station)
        {
            StationToList nStation = new StationToList();
            nStation.Id = station.Id;
            nStation.Name = station.NameStation;
            foreach (var drone in lDroneToList)
            {
                if (drone.DStatus == Maintance && equalLocations(drone.CurrLocation, station))
                {
                    ++nStation.FullPositions;
                }
            }
            nStation.AvailablePositions = station.NumAvailablePositions - nStation.FullPositions;
            return nStation;
        }

        private bool equalLocations(Location location, IDal.DO.BaseStation station)
        {
            return location.Longitude == station.Longitude && location.Latitude == station.Latitude;
        }
        
        private IDal.DO.BaseStation closestStation(Location location)
        {
            var stationDalList = dal.GetListFromDal<IDal.DO.BaseStation>();
            var cCoord = new geoCoordinate(location);
            var sCoord = new geoCoordinate(stationDalList.ElementAt(0).Latitude, stationDalList.ElementAt(0).Longitude);
            double currDistance, distance = sCoord.GetDistanceTo(cCoord);
            int index = 0;
            for (int i = 1; i < stationDalList.Count(); i++)
            {
                sCoord = new geoCoordinate(stationDalList.ElementAt(i).Latitude, stationDalList.ElementAt(i).Longitude);
                currDistance = sCoord.GetDistanceTo(cCoord);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    index = i;
                }
            }
            return stationDalList.ElementAt(index);
        }

        public void AddingBaseStation(Station bLStation)
        {
            if (dal.ExistsInBaseStation(bLStation.Id))
            {
                throw new Exception("The id is already exists in the base station list!");
            }

            BaseStation station = new BaseStation() { Id = bLStation.Id,
                Latitude = bLStation.SLocation.Latitude, Longitude = bLStation.SLocation.Longitude,
                NameStation = bLStation.NameStation,
                NumberOfChargingPositions = bLStation.NumAvailablePositions };
            dal.AddingBaseStation(station);
        }
    }
}
