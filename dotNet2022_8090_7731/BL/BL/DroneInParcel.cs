using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// A class of DroneInParcel that contains:
    /// Id
    /// BatteryStatus
    ///CurrLocation
    /// </summary>
    public class DroneInParcel
    {
        public DroneInParcel(int id, double batteryStatus, Location location)
        {
            Id = id;
            BatteryStatus = batteryStatus;
            CurrLocation = location;
        }
        public int Id{ get; set; }
        public double BatteryStatus{ get; set; }
        public Location CurrLocation { get; set; }
        
       
    }

}
