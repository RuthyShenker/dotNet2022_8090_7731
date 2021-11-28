using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// A class of ChargingDrone that contains:
    /// DroneId
    ///BatteryStatus
    /// </summary>
    public class ChargingDrone
    {
        public ChargingDrone()
        {
        }

        /// <summary>
        /// A constructor of ChargingDrone with fields.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="batteryStatus"></param>
        public ChargingDrone(int droneId,double batteryStatus)
        {
            DroneId = droneId;
            BatteryStatus = batteryStatus;
        }
        public ChargingDrone():this(0,0)
        {
           
        }
        public int DroneId { get; set; }
        public double BatteryStatus { get; set; }
    }
}
