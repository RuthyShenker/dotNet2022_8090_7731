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
        /// A constructor of Drone with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="weight"></param>
        /// <param name="batteryStatus"></param>
        /// <param name="droneStatus"></param>
        /// <param name="pInTransfer"></param>
        /// <param name="currLocation"></param>
        //public Drone(int id, string model, WeightCategories weight, double batteryStatus, DroneStatus droneStatus, ParcelInTransfer pInTransfer, Location currLocation)
        //{
        //    Id = id;
        //    Model = model;
        //    Weight = weight;
        //    BatteryStatus = batteryStatus;
        //    DroneStatus = droneStatus;
        //    PInTransfer = pInTransfer;
        //    CurrLocation = currLocation;
        //}

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
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}

