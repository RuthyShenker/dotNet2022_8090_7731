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
        /// <summary>
        /// A constructor of DroneInParcel with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="batteryStatus"></param>
        /// <param name="location"></param>
        public DroneInParcel(int id, double batteryStatus, Location location)
        {
            Id = id;
            BatteryStatus = batteryStatus;
            CurrLocation = location;
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id{ get; init; }
        public double BatteryStatus{ get; set; }
        public Location CurrLocation { get; set; }
        
       
    }

}
