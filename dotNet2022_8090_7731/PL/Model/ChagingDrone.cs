using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ChargingDrone : ObservableBase
    {
        public int DroneId { get; set; }
        public double BatteryStatus { get; set; }

        public ChargingDrone(int droneId, double batteryStatus)
        {
            DroneId = droneId;
            BatteryStatus = batteryStatus;
        }

        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}
