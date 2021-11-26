using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ChargingDrone
    {
        public ChargingDrone()
        {
        }

        public ChargingDrone(int droneId,double batteryStatus)
        {
            DroneId = droneId;
            BatteryStatus = batteryStatus;
        }
        public int DroneId { get; set; }
        public double BatteryStatus { get; set; }
    }
}
