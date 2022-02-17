using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    /// A  public class ChargingDrone impliments:ObservableBase
    /// includes:
    /// DroneId
    /// BatteryStatus

    /// </summary>
    public class ChargingDrone : ObservableBase
    {
        public int DroneId { get; set; }
        public double BatteryStatus { get; set; }

        /// <summary>
        /// A constructor of ChargingDrone with params
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="batteryStatus"></param>
        public ChargingDrone(int droneId, double batteryStatus)
        {
            DroneId = droneId;
            BatteryStatus = batteryStatus;
        }

        /// <summary>
        /// A function that returns the datails of ChargingDrone.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}
