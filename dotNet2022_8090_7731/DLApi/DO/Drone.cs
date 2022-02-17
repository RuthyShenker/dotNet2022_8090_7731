
using System;
using System.Collections.Generic;


namespace DO
{
    /// <summary>
    /// A struct of Drone that impliments IIdentifiable, IDalDo, constains:
    /// Id, Model, MaxWeight.
    /// </summary>
    [Serializable]
    public struct Drone : IIdentifiable, IDalDo
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
