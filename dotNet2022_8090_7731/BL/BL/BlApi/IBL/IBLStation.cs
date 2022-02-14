using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBLStation
    {
        //ADD:
        public void AddingBaseStation(int id, string name, double longitude, double latitude, int numPositions);

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
