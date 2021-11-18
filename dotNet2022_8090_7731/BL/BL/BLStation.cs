using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    partial class BL
    {
        public IEnumerable<StationToList> GetStations()
        {
            return GetBList<StationToList, IDal.DO.BaseStation>();

            IEnumerable<StationToList> bStationsList = new List<StationToList>();
            List<IDal.DO.Station> dStationsList = GetList<IDal.DO.BaseStation>();
            //IEnumerable<IDal.DO.Station> dParcelList = dal.GetStations();
            foreach (var station in dStationsList)
            {
                bStationsList.Add(Map(station));
            }
            return bStationsList;
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

        private StationToList Map(IDal.DO.BaseStation station)
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

        private Location closestStation(IDal.DO.Customer customer, IEnumerable<BaseStation> stationDalList)
        {
            var cCoord = new GeoCoordinate(customer.Latitude, customer.Longitude);
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
            return new Location(stationDalList.ElementAt(index).Latitude, stationDalList.ElementAt(index).Longitude);
        }

    }
}
