using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL.BO
{
    public class Drone
    {
         public Drone(int id, int model,WeightCategories weight, float batteryStatus, DroneStatus droneStatus, ParcelInTransfer pInTransfer, Location currLocation)
        {
            Id = id;
            Model = model;
            Weight = weight;
            BatteryStatus = batteryStatus;
            DroneStatus = droneStatus;
            PInTransfer = pInTransfer;
            CurrLocation = currLocation;
        }
        int Id { get; init; }
        string Model { get; set; }
        WeightCategories Weight { get; set; }
        float BatteryStatus { get; set; }
        DroneStatus DroneStatus { get; set; }
        ParcelInTransfer PInTransfer { get; set; }
        Location CurrLocation { get; set; } 
       
    }
}
