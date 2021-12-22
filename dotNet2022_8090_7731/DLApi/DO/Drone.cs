using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    ///// <summary>
    ///// A struct that of Drone constains:
    ///// Id, Model, MaxWeight, BatteryStatus, Status.
    ///// </summary>
    public struct Drone : IIdentifiable, IDalObject
    {
        /// <summary>
        /// this field is init
        /// </summary>
        public int Id { get; init; }

        public string Model { get; set; }

        public WeightCategories MaxWeight { get; set; }

        /// <summary>
        /// A function that returns the details of the Drone
        /// </summary>
        /// <returns>The details</returns>
        public override string ToString()
        {
            return $"Id: {Id}   Model: {Model}    MaxWeight: {MaxWeight}    ";
        }
    }
}
