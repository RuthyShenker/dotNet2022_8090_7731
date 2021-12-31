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
        public void AddingBaseStation(int id, string name, double longitude, double latitude, int numPositions);

        void UpdatingStationDetails(int stationId, string stationName, int amountOfPositions);
        IEnumerable<StationToList> GetStations();
        IEnumerable<StationToList> AvailableSlots();
        Station GetStation(int stationId);
        string DeleteStation(int stationId);
    }
}
