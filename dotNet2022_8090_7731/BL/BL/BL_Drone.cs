using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;

namespace IBL.BO
{
    class BL_Drone
    {
        int Id { get; init; }
        string Model { get; set; }
        WeightCategories Weight { get; set; }
        float BatteryStatus { get; set; }
        DroneStatus DStatus { get; set; }
        ParcelInTransfer PInTransfer { get; set; }
        Location CurrLocation { get; set; } 
    }
}
