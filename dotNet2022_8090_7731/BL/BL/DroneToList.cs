using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// A class of DroneToList
    /// that contains:
    /// Id
    ///Model
    ///Weight
    ///BatteryStatus
    ///DStatus
    ///CurrLocation
    ///NumOfParcel
    /// </summary>
    public class DroneToList
    {
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double BatteryStatus { get; set; }
        public DroneStatus DStatus { get; set; }
        public Location CurrLocation { get; set; }
        public int? NumOfParcel{ get; set; }
       
    }
}
