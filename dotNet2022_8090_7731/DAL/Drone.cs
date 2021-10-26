using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        /// <summary>
        /// A struct that of Drone constains:
        /// Id, Model, MaxWeight, BatteryStatus, Status.
        /// </summary>
        public struct Drone
        {

            /// <summary>
            /// A constructor of Drone that gets parameters 
            /// and initalizes the new instance with this 
            /// parameters.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="model"></param>
            /// <param name="maxWeight"></param>
            /// <param name="batteryStatus"></param>
            /// <param name="status"></param>
            public Drone(int id, string model, WeightCategories maxWeight, double batteryStatus, DroneStatuses status)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
                BatteryStatus = batteryStatus;
                Status = status;
            }

            /// <summary>
            /// A constuctor of Drone that gets 
            /// an instance of Drone and initalizes
            /// the new instance with the parameters of this instance.
            /// </summary>
            /// <param name="drone"></param>
            public Drone(Drone drone)
            {
                Id = drone.Id;
                Model = drone.Model;
                MaxWeight = drone.MaxWeight;
                BatteryStatus = drone.BatteryStatus;
                Status = drone.Status;
            }
            /// <summary>
            /// this field is init
            /// </summary>
            public int Id { get; init; }

            public string Model { get; set; }

            public WeightCategories MaxWeight { get; set; }

            public double BatteryStatus { get; set; }

            public DroneStatuses Status { get; set; }
           
            /// <summary>
            /// A function that returns the details of the Drone
            /// </summary>
            /// <returns>The details</returns>
            public override string ToString()
            {
                return $"Id: {Id}   Model: {Model}    MaxWeight: {MaxWeight}    " +
                    $"BatteryStatus: {BatteryStatus}    Status: {Status}";
            }
            /// <summary>
            /// A function that returns a new drone initalizes 
            /// with the drone that the function worked on.
            ///</summary>
            /// <returns></returns>
            public Drone Clone()
            {
                return new Drone()
                {
                    Id = Id,
                    Model= Model,
                    MaxWeight= MaxWeight,
                    BatteryStatus= BatteryStatus,
                    Status= Status
                };
            }

        }
    }
}
