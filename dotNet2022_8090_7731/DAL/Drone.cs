using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public struct Drone
        {
            public string Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatus { get; set; }
            public DroneStatuses Status { get; set; }
            public Drone(string id, string model, WeightCategories maxWeight, double batteryStatus, DroneStatuses status)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
                BatteryStatus = batteryStatus;
                Status = status;
            }
            public override string ToString()
            {
                return $"Id: {Id}   Model: {Model}    MaxWeight: {MaxWeight}    " +
                    $"BatteryStatus: {BatteryStatus}    Status: {Status}";
            }

        }
    }
}
