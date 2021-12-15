using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
        public DroneToList()
        {

        }
        public DroneToList(int id, string model, WeightCategories weight
            , double batteryStatus, DroneStatus dStatus, Location currLocation, int? numOfParcel)
        {
            Id = id;
            Model = model;
            Weight = weight;
            BatteryStatus = batteryStatus;
            DStatus = dStatus;
            CurrLocation = currLocation;
            DeliveredParcelId = numOfParcel;
        }
        public DroneToList(int id, string model, WeightCategories weight):this(id,model,weight,0,0,null,0)
        {
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double BatteryStatus { get; set; }
        public DroneStatus DStatus { get; set; }
        public Location CurrLocation { get; set; }
        public int? DeliveredParcelId{ get; set; }

        public override string ToString()
        {
            //return string.Format("Id: {0}" ,"Model: {1}", "Weight: {2}", "BatteryStatus: {3}", "DStatus: {4}",
            //    "longitude: {5}", "latitude: {6}", "DeliveredParcelId: {7}"
            //    ,Id,Model,Weight,BatteryStatus,DStatus,CurrLocation.Longitude,CurrLocation.Latitude,DeliveredParcelId);
            //TODO:
            return BL.Tools.ToStringProps(this);
        }

    }
}
