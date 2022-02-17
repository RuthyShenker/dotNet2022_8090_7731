using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    ///A class of Drone contains:
    ///Id
    ///Model
    ///Weight
    ///BatteryStatus
    ///DroneStatus
    ///ParcelInTransfer
    ///CurrLocation
    /// </summary>
    public class Drone
    {
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double BatteryStatus { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public ParcelInTransfer PInTransfer { get; set; }
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

