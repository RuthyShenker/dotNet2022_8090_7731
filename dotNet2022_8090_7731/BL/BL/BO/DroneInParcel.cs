using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public double BatteryStatus { get; set; }
        public Location CurrLocation { get; set; }

        /// <summary>
        /// A function that returns the details of this Drone
        /// </summary>
        /// <returns> the details of this Drone</returns>
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}


