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
    }
}
