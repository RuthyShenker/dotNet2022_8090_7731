using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class EditStation
    {
        public EditStation(int id, string name, int numAvailablePositions, Location location,
            IEnumerable<BO.ChargingDrone> listChargingDrone)
        {
            Id = id;
            Name = name;
            NumAvailablePositions = numAvailablePositions;
            Location = location;
            ListChargingDrone = listChargingDrone;
        }
        public int Id { get; init; }
        public string Name { get; set; }
        public int NumAvailablePositions { get; set; }
        public Location Location { get; init; }
        // two lists
        public IEnumerable<BO.ChargingDrone> ListChargingDrone { get; set; }
    }
}
