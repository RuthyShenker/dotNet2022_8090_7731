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
        void AddingBaseStation(Station baseStation);
        void UpdatingStationDetails(int stationId, string stationName, string amountOfPositions);
        IEnumerable<StationToList> GetStations();
        IEnumerable<StationToList> AvailableSlots();
        Station GetStation(int stationId);
    }
}
