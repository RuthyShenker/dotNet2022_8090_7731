using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    /// <summary>
    /// interface IBLStation includes:
    /// AddingBaseStation
    /// UpdatingStationDetails
    /// GetStation
    /// GetStations
    /// AvailableSlots
    /// DeleteStation
    /// </summary>
    public interface IBLStation
    {
        //ADD:
        public void AddingBaseStation(BO.Station station);

        //UPDATE:
        void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions);

        //GET:
        Station GetStation(int stationId);
        IEnumerable<StationToList> GetStations();
        IEnumerable<StationToList> AvailableSlots(int numPositions = 0);
        
        //DELETE:
        string DeleteStation(int stationId);
    }
}
