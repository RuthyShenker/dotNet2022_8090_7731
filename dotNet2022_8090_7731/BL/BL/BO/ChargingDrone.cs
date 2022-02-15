using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// A class of ChargingDrone that contains:
    /// DroneId
    ///BatteryStatus
    /// </summary>
    public class ChargingDrone
    {
        public int DroneId { get; set; }
        public double BatteryStatus { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}

