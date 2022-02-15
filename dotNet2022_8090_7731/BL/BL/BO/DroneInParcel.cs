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
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}


